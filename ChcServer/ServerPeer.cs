using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChcServer
{
    /// <summary>
    /// 服务器端封装
    /// </summary>
    public class ServerPeer
    {
        /// <summary>
        /// 新连接的客户端
        /// </summary>
        private Socket clientsocket = null;

        /// <summary>
        /// 限制最大连接数量的信号量
        /// </summary>
        private Semaphore semaphore = null;

        /// <summary>
        /// 客户端对象池
        /// </summary>
        private ClientPeerPool clientPeerPool = null;

        public ServerPeer()
        {
            clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(int Port , int MaxCount)//端口号 最大人数
        {   
            try
            {
                semaphore = new Semaphore(MaxCount, MaxCount);
                clientsocket.Bind(new IPEndPoint(IPAddress.Any, Port));
                clientsocket.Listen(10);
                //挂起等待连接的最大数量

                clientPeerPool = new ClientPeerPool(MaxCount);
                for(int i = 0; i < MaxCount; i++)
                {
                    ClientPeer tempClient = new ClientPeer();
                    clientPeerPool.EnqueuePool(tempClient);

                    tempClient.ReceiveComplatedEvent += PacketReceiveComplatedEvent;
                    //数据包解析完成后的回调

                    tempClient.ReceiveArgs.Completed += receive_Complateed;
                    //数据接收异步操作接受完成后的操作
                }

                startAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        




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
            semaphore.WaitOne();//若达到最大连接数量则阻止该线程

            bool result = clientsocket.AcceptAsync(e);
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
            //Socket clientsocket = e.AcceptSocket;
            ClientPeer clientPeer = clientPeerPool.DequeuePool();//取出对象池中的对象
            clientPeer.Clientsocket = e.AcceptSocket;

            //开始接受数据
            startReceive(clientPeer);

            e = null;
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
                    //TODO
                }
                else
                {
                    //异常断开
                    //TODO
                }
            }
        }

        /// <summary>
        /// 当异步操作事件完成后触发的操作
        /// </summary>
        /// <param name=""></param>
        private void receive_Complateed(object sender , SocketAsyncEventArgs e)
        {
            processAccept(e);
        }

        /// <summary>
        /// 一个数据包解析完成后的回调方法
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="value"></param>
        private void PacketReceiveComplatedEvent(ClientPeer clientPeer, SocketMgr value)
        {
            //TODO
            throw new NotImplementedException();
        }
        #endregion

        #region 断开连接

        #endregion

        #region 发送数据

        #endregion
    }
}
