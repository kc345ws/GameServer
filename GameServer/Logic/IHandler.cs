using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;

namespace GameServer.Logic
{
    /// <summary>
    /// 所有逻辑层模块都要可以接收和断开
    /// </summary>
    public interface IHandler
    {
        void OnDisConnect(ClientPeer clientPeer);

        void OnReceive(ClientPeer clientPeer, int subcode , object value);
    }
}
