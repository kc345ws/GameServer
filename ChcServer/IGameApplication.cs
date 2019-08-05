using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ChcServer
{
    /// <summary>
    /// 游戏应用层
    /// </summary>
    public interface IGameApplication
    {
        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="mgr"></param>
        void OnReceive(ClientPeer clientPeer , SocketMsg mgr);


        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="clientPeer"></param>
        void OnDisConnect(ClientPeer clientPeer);
    }
}
