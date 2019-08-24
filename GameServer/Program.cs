using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChcServer;
using GameServer.DataBase;
using GameServer.Modle;
using MySql.Data.MySqlClient;
using Protocol.Constants.Orc;

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
