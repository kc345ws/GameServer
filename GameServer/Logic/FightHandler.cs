using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;

namespace GameServer.Logic
{
    /// <summary>
    /// 游戏战斗处理类
    /// </summary>
    public class FightHandler : IHandler
    {
        private static FightHandler instance = new FightHandler();
        public static FightHandler Instance { get
            {
                lock (instance)
                {
                    if(instance == null)
                    {
                        instance = new FightHandler();
                    }
                    return instance;
                }
            } }

        private FightHandler()
        {
            MatchHandler.Instance.StartGameEvent += startGame;
        }

        public void OnDisConnect(ClientPeer clientPeer)
        {

        }

        public void OnReceive(ClientPeer clientPeer, int subcode, object value)
        {
            switch (subcode)
            {

            }
        }

        private void startGame()
        {

        }
    }
}
