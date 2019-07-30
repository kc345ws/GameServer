using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChcServer
{
    /// <summary>
    /// 客户端对象池
    /// </summary>
    class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeers = null;

        
        public ClientPeerPool(int num)
        {     
            clientPeers = new Queue<ClientPeer>();   
        }


        /// <summary>
        /// 将新连接的客户端加入对象池末尾
        /// </summary>
        /// <param name="clientPeer"></param>
        public void EnqueuePool(ClientPeer clientPeer)
        {
            clientPeers.Enqueue(clientPeer);
        }

        /// <summary>
        /// 移除最开始的客户端对象，并返回结果
        /// </summary>
        public ClientPeer DequeuePool()
        {
            return clientPeers.Dequeue();
        }
    }
}
