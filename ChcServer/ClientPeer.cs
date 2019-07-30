using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChcServer
{
    /// <summary>
    /// 客户端的封装连接对象
    /// </summary>
    class ClientPeer
    {
        public ClientPeer()
        {
            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.UserToken = this; 
        }
        /// <summary>
        /// 解析完一条数据后的回调
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="value"></param>
        public delegate void ReceiveComplated(ClientPeer clientPeer, SocketMgr value);
        public event ReceiveComplated ReceiveComplatedEvent;
        /// <summary>
        /// 防止并发操作的信号量
        /// </summary>
        bool isProcessing = false;
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
        private List<byte> clientdataCache = new List<byte>();
        //需要解决粘包和拆包问题

            /// <summary>
            /// 开始接受数据
            /// </summary>
            /// <param name="data"></param>
        public void StartReceive(byte[] packet)
        {
            clientdataCache.AddRange(packet);
            if (isProcessing == false)
            {
                processReceive();
            }
        }

        private void processReceive()
        {
            isProcessing = true;  
            byte[] data = EncodeTool.DecodeMessage(ref clientdataCache);
            if (data == null)
            {
                isProcessing = false;
                return;
            }

            SocketMgr value = EncodeTool.DeCodeSocketMgr(data);
            //TODO 将解析后的数据转化为具体的类型

            //回调给上层
            if(ReceiveComplatedEvent != null)
            {
                ReceiveComplatedEvent(this, value);
            }

            //递归处理数据包
            processReceive();
        }
    }

    
}
