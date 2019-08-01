using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
using Protocol.Code;
using Protocol.Models;

namespace GameServer.Logic
{
    class AccountHandler : IHandler
    {
        public void OnDisConnect(ClientPeer clientPeer)
        {
            throw new NotImplementedException();
        }

        public void OnReceive(ClientPeer clientPeer, int subcode, object value)
        {
            switch (subcode)
            {
                case AccountCode.LOGIN_CREQ:
                    AccountModle account = value as AccountModle;
                    Console.WriteLine(account.Account);
                    Console.WriteLine(account.Password);
                    break;
                case AccountCode.REGISTER_CREQ:
                    break;
            }
        }
    }
}
