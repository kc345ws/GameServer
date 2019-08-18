using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net;
using System.Net.Sockets;
using System.Threading;



namespace ChcServer
{
    //public delegate void AccountOfflineDelegate(ClientPeer clientPeer);//帐号下线
    /// <summary>
    /// 服务器端封装
    /// </summary>
    public class ServerPeer
    {
        //public event AccountOfflineDelegate accountOfflineEvent;
        #region 变量及构造函数
        /// <summary>
        /// 服务器端套接字
        /// </summary>
        private Socket serversocket = null;

        /// <summary>
        /// 限制最大连接数量的信号量
        /// </summary>
        private Semaphore semaphore = null;

        /// <summary>
        /// 客户端对象池
        /// </summary>
        private ClientPeerPool clientPeerPool = null;

        /// <summary>
        /// 游戏应用层的抽象
        /// </summary>
        private IGameApplication application;

        /// <summary>
        /// 设置抽象层
        /// </summary>
        /// <param name="app"></param>
        public void SetApplication(IGameApplication app)
        {
            application = app;
        }

        public ServerPeer()
        {
           serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion

        #region 启动服务器
        public void Start(int Port, int MaxCount)//端口号 最大人数
        {
            try
            { 
                semaphore = new Semaphore(MaxCount, MaxCount);        
                //挂起等待连接的最大数量
                clientPeerPool = new ClientPeerPool(MaxCount);

                ClientPeer tempClient = null;
                for (int i = 0; i < MaxCount; i++)
                {
                    tempClient = new ClientPeer();

                    tempClient.ReceiveComplatedEvent += PacketReceiveComplatedEvent;
                    //数据包解析完成后的回调

                    tempClient.ReceiveArgs.Completed += receive_Complateed;
                    //数据接收异步操作接受完成后的操作

                    tempClient.SendFailEvent += DisConttect;
                    //注册向客户端发送数据失败的事件

                    clientPeerPool.EnqueuePool(tempClient);
                }

                serversocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                serversocket.Listen(10);
                Console.WriteLine("服务器启动成功");

                startAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        #endregion

        #region 接受连接
        private void startAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                //委托的多播 形成一个调用列表 调用时按顺序调用其中的方法
                //事件生成时会调用委托
                e.Completed += accept_Completed;//为事件注册方法
            }
            //semaphore.WaitOne();//若达到最大连接数量则阻止该线程

            //开启一个新的线程
            bool result = serversocket.AcceptAsync(e);
            //判断异步事件是否执行完毕
            //结果: 1.被挂起没有执行完毕交给别的线程来执行  2.执行完毕直接执行完毕

            //返回true表示被挂起没有完成，当异步事件完成后会触发Completed事件，事件又会调用委托
            //返回false表示连接成功直接进行后续操作
            if (result == false)
            {
                processAccept(e);
                //若结果为false则代表连接成功，直接执行后续操作
            }
        }

        //当异步事件触发时调用
        private void accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            processAccept(e);
        }

        /// <summary>
        /// 连接成功后进行处理
        /// </summary>
        /// <param name="e">SocketAsyncEventArgs</param>
        private void processAccept(SocketAsyncEventArgs e)
        {
            semaphore.WaitOne();//若达到最大连接数量则阻止该线程
            //Socket clientsocket = e.AcceptSocket;
            ClientPeer clientPeer = clientPeerPool.DequeuePool();//取出对象池中的对象
            clientPeer.Clientsocket = e.AcceptSocket;

            if (clientPeer.Clientsocket != null)
            {
                Console.WriteLine("客户端连接成功 :" + clientPeer.Clientsocket.RemoteEndPoint.ToString());
            }

            if (clientPeer != null)
            {
                startReceive(clientPeer);
            }


            //e = null;
            e.AcceptSocket = null;
            startAccept(e);//递归调用，处理其他连接  
        }
        #endregion

        #region 接受数据
        /// <summary>
        /// 开始接受数据
        /// </summary>
        /// <param name="e"></param>
        private void startReceive(ClientPeer clientPeer)
        {
            try
            {
                //clientPeer.ReceiveArgs.Completed += receive_Complateed;//为异步事件注册方法

                //开始异步接受数据
                

                bool result = clientPeer.Clientsocket.ReceiveAsync(clientPeer.ReceiveArgs);
                
                if(result == false)
                {
                    processReceive(clientPeer.ReceiveArgs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 处理接受的数据
        /// </summary>
        /// <param name="e"></param>
        private void processReceive(SocketAsyncEventArgs e)
        {
            ClientPeer clientPeer = e.UserToken as ClientPeer;
            if(e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                //如果接受成功且数据长度大于0
                byte[] data = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesTransferred);
                clientPeer.StartReceive(data);

                startReceive(clientPeer);//递归接接收其他客户端数据
            }else if (e.BytesTransferred == 0)//数据长度为0 则为断开
            {
                if(e.SocketError == SocketError.Success)
                {
                    //正常断开
                    DisConttect(clientPeer, "客户端正常断开");           
                }
                else
                {
                    //异常断开
                    DisConttect(clientPeer, e.SocketError.ToString());
                }
            }
        }

        /// <summary>
        /// 当异步操作事件完成后触发的操作
        /// </summary>
        /// <param name=""></param>
        private void receive_Complateed(object sender , SocketAsyncEventArgs e)
        {
            processReceive(e);
        }

        /// <summary>
        /// 一个数据包解析完成后的回调方法
        /// </summary>
        /// <param name="clientPeer">连接对象</param>
        /// <param name="value">消息</param>
        private void PacketReceiveComplatedEvent(ClientPeer clientPeer, SocketMsg value)
        {
            //给应用层数据，让其使用
            application.OnReceive(clientPeer, value);
        }
        #endregion

        #region 断开连接

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="clientPeer">客户端连接对象</param>
        /// <param name="reason">断开原因</param>
        public void DisConttect(ClientPeer clientPeer,string reason)
        {
            try
            {

                if(clientPeer == null)
                {
                    Console.WriteLine("服务器断开连接,原因:"+ "连接对象为空，无法断开连接");
                    throw new Exception("连接对象为空，无法断开连接");
                }

                if (clientPeer.Clientsocket != null)
                {
                    Console.WriteLine(clientPeer.Clientsocket.RemoteEndPoint.ToString() + "客户端断开连接，原因:" + reason);
                }
                //账号下线
                //accountOfflineEvent?.Invoke(clientPeer);

                //应用层断开连接
                application.OnDisConnect(clientPeer);

                clientPeer.Disconnect();//客户端自身处理断开操作


                clientPeerPool.EnqueuePool(clientPeer);//回收客户端连接对象

                

                semaphore.Release();//释放信号量
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        #endregion

        #region 发送数据

        #endregion
    }
}
