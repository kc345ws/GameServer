﻿using System;
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
        private IHandler account = new AccountHandler();
        
        //断开连接
        public void OnDisConnect(ClientPeer clientPeer)
        {
            account.OnDisConnect(clientPeer);
            UserHandler.Instance.OnDisConnect(clientPeer);
        }

        public void OnReceive(ClientPeer clientPeer, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    account.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;

                case OpCode.USER:
                    UserHandler.Instance.OnReceive(clientPeer, msg.SubCode, msg.Value);
                    break;
            }
        }
    }
}
