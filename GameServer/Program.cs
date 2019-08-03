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
    public class Program
    {
        public static ServerPeer Server { get; private set; }
        static void Main(string[] args)
        {
            
            Server = new ServerPeer();
            Server.SetApplication(new NetMsgCenter());
            Server.Start(59800, 100);


            Console.ReadKey();
        }
    }
}
