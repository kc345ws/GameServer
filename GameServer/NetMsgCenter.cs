using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChcServer;
using GameServer.Logic;
using Protocol.Code;

namespace GameServer
{
    /// <summary>
    /// 网络数据转发中心
    /// 只转发不处理
    /// </summary>
    public class NetMsgCenter : IGameApplication
    {
        //private IHandler account = new AccountHandler();
        
        //断开连接
        public void OnDisConnect(ClientPeer clientPeer)
        {
            //FightHandler.Instance.OnDisConnect(clientPeer);
            //ChatHandler.Instance.OnDisConnect(clientPeer);
            MatchHandler.Instance.OnDisConnect(clientPeer);
            UserHandler.Instance.OnDisConnect(clientPeer);
            AccountHandler.Instance.OnDisConnect(clientPeer);


            //account.OnDisConnect(clientPeer);    
        }

        public void OnReceive(ClientPeer clientPeer, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    AccountHandler.Instance.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;

                case OpCode.USER:
                    UserHandler.Instance.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;

                case OpCode.MATCH:
                    MatchHandler.Instance.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;

                case OpCode.CHAT:
                    ChatHandler.Instance.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;

                case OpCode.FIGHT:
                    FightHandler.Instance.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;
            }
        }
    }
}
