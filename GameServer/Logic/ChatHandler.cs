using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
using Protocol.Code;
using GameServer.Cache;
using GameServer.Cache.Match;
using Protocol.Dto.Fight;
using GameServer.Model;

namespace GameServer.Logic
{
    public class ChatHandler : IHandler
    {
        private static ChatHandler instance = new ChatHandler();
        public static ChatHandler Instance { get
            {
                lock (instance)
                {
                    if(instance == null)
                    {
                        instance = new ChatHandler();
                    }
                    return instance;
                }
            } }

        private ChatDto chatDto;
        private ChatHandler()
        {
            chatDto = new ChatDto();
        }
        

        public void OnDisConnect(ClientPeer clientPeer)
        {
            UserModel userModel = UserCache.Instance.GetModelByClientPeer(clientPeer);
            Console.WriteLine("玩家:" + userModel.Name + "从聊天服务器掉线");
        }

        public void OnReceive(ClientPeer clientPeer, int subcode, object value)
        {
            switch (subcode)
            {
                case ChatCode.CREQ:
                    processCREQ(clientPeer, (int)value);
                    break;
            }
        }

        private void processCREQ(ClientPeer clientPeer , int chattype)
        {
            if (!UserCache.Instance.IsOnline(clientPeer))
            {
                return;
            }

            int uid = UserCache.Instance.GetId(clientPeer);

            if (MatchCache.Instance.IsMatching(uid))
            {
                //如果用户正在匹配房间可以发送聊天信息
                MatchRoom matchRoom = MatchCache.Instance.GetRoom(uid);
                chatDto.Change(uid, chattype);
                //向自己以外的玩家发送聊天消息
                matchRoom.Broadcast(OpCode.CHAT, ChatCode.SBOD, chatDto);
            }
            //TODO在游戏房间也可以发送
        }
    }
}
