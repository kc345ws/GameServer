using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace ChcServer
{
    /// <summary>
    /// 客户端的封装连接对象
    /// </summary>
    public class ClientPeer
    {
        public ClientPeer()
        {
            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.UserToken = this;
            //ReceiveArgs.SetBuffer(new byte[1024], 0, 1024);
            ReceiveArgs.SetBuffer(new byte[4096], 0, 4096);
            clientdataCache = new List<byte>();
            sendQueue = new Queue<byte[]>();
            SendArgs = new SocketAsyncEventArgs();
            SendArgs.Completed += SendArgs_Completed;     
            
            
        }

        /// <summary>
        /// 解析完一条数据后的回调
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="value"></param>
        public delegate void ReceiveComplated(ClientPeer clientPeer, SocketMsg value);
        public ReceiveComplated ReceiveComplatedEvent;

        /// <summary>
        /// 防止并发接收操作的信号量
        /// </summary>
        bool isReceiveProcessing = false;

        /// <summary>
        /// 客户端的Socket对象
        /// </summary>
        public Socket Clientsocket { get; set; }

        /// <summary>
        /// 接受客户端数据的异步操作对象
        /// </summary>
        public SocketAsyncEventArgs ReceiveArgs { get; set; }

        /// <summary>
        /// 客户端传送的数据
        /// </summary>
        private List<byte> clientdataCache = null;
        //需要解决粘包和拆包问题



        #region 接受数据

        /// <summary>
        /// 开始接受数据
        /// </summary>
        /// <param name="data"></param>

        public void StartReceive(byte[] packet)
        {
            clientdataCache.AddRange(packet);
            if (isReceiveProcessing == false)
            {
                processReceive();
            }
        }

        private void processReceive()
        {
            isReceiveProcessing = true;
            byte[] data = EncodeTool.DecodeMessage(ref clientdataCache);
            if (data == null)
            {
                isReceiveProcessing = false;
                return;
            }

            SocketMsg value = EncodeTool.DeCodeSocketMgr(data);
            //将解析后的数据转化为具体的类型

            //回调给上层
            if (ReceiveComplatedEvent != null)
            {
                ReceiveComplatedEvent(this, value);
            }

            //递归处理数据包
            processReceive();
        }
        #endregion

        #region 断开连接

        public void Disconnect()
        {
            Clientsocket.Shutdown(SocketShutdown.Both);
            Clientsocket.Close();
            //清空数据
            Clientsocket = null;
            isReceiveProcessing = false;
            clientdataCache.Clear();
        }



        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据时的数据队列
        /// </summary>
        private Queue<byte[]> sendQueue;

        /// <summary>
        /// 防止发送数据并发操作的信号量
        /// </summary>
        private bool isSendProcess = false;

        /// <summary>
        /// 套接字发送数据时的操作对象
        /// </summary>
        private SocketAsyncEventArgs SendArgs;

        /// <summary>
        /// 发送数据失败 回调给上层的委托
        /// </summary>
        public delegate void SendFailDG(ClientPeer clientPeer, string reason);
        public event SendFailDG SendFailEvent;

        /// <summary>
        /// 开始发送数据
        /// </summary>
        /// <param name="Opcode">操作码</param>
        /// <param name="Subcode">子操作</param>
        /// <param name="value">参数</param>
        public void StartSend(int Opcode , int Subcode , object value)
        {
            SocketMsg mgr = new SocketMsg(Opcode, Subcode, value);
            byte[] data = EncodeTool.EncodeSocketMgr(mgr);
            byte[] packet = EncodeTool.EncodeMessage(data);//构造数据包

            StartSend(packet);
        }

        /// <summary>
        /// 广播消息时的性能优化处理
        /// </summary>
        /// <param name="packet"></param>
        public void StartSend(byte[] packet)
        {
            sendQueue.Enqueue(packet);

            if (!isSendProcess)
            {
                sendPacket();
            }
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        private void sendPacket()
        {
            isSendProcess = true;

            if(sendQueue.Count == 0)//递归结束条件
            {
                isSendProcess = false;
                return;
            }

            byte[] packet = sendQueue.Dequeue();
            SendArgs.SetBuffer(packet, 0, packet.Length);
            bool result = Clientsocket.SendAsync(SendArgs);//异步发送消息

            if (!result)
            {
                processSend();
            }
        }

        private void processSend()
        {
            if(SendArgs.SocketError != SocketError.Success)
            {
                //数据发送失败 说明客户端断开连接了
                //触发发送数据失败的事件
                SendFailEvent(this, SendArgs.SocketError.ToString());
            }
            else
            {
                //数据发送成功 客户端没有断开连接 递归调用发送数据
                sendPacket();
            }
        }

        /// <summary>
        /// 异步发送事件完成时触发的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            processSend();
        }

        #endregion
    }
}
