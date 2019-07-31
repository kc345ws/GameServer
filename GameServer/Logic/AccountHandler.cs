using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;

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
            throw new NotImplementedException();
        }
    }
}
