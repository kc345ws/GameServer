using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;

namespace GameServer
{
    /// <summary>
    /// 应用层
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            server.SetApplication(new NetMsgCenter());
            server.Start(59800, 100);

            Console.ReadKey();
        }
    }
}
