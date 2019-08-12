using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
using Protocol.Code;
using GameServer.Cache;
using GameServer.Cache.Match;
using Protocol.Dto;
using ChcServer.Util.Concurrent;
using GameServer.Model;

namespace GameServer.Logic
{
    public delegate void StartGameDelegate(List<int> uidList);
    /// <summary>
    /// 匹配逻辑层
    /// </summary>
    public class MatchHandler : IHandler
    {
        public event StartGameDelegate StartGameEvent;

        private static MatchHandler instance = new MatchHandler();

        public static MatchHandler Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                    {
                        instance = new MatchHandler();
                    }
                    return instance;
                }
            }
        }
        
        private MatchHandler() { }
        /// <summary>
        /// 断线
        /// </summary>
        /// <param name="clientPeer"></param>
        public void OnDisConnect(ClientPeer clientPeer)
        {
            if (!UserCache.Instance.IsOnline(clientPeer))
            {
                return;
            }
            int uid = UserCache.Instance.GetId(clientPeer);
            if (MatchCache.Instance.IsMatching(uid))
            {
                //MatchCache.Instance.Leave(uid);
                leave(clientPeer);
            }
        }

        public void OnReceive(ClientPeer clientPeer, int subcode, object value)
        {
            switch (subcode)
            {
                case MatchCode.ENTER_CREQ:
                    enter(clientPeer);
                    break;

                case MatchCode.LEAVE_CREQ:
                    leave(clientPeer);
                    break;

                case MatchCode.READY_CREQ:
                    ready(clientPeer);
                    break;

            }
        }

        private void enter(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(() =>
            {
                if (!UserCache.Instance.IsOnline(clientPeer))
                {
                    return;
                }

                int uid = UserCache.Instance.GetId(clientPeer);
                if (MatchCache.Instance.IsMatching(uid))
                {
                    //如果用户正在匹配
                    //clientPeer.StartSend(OpCode.MATCH, MatchCode.ENTER_SRES, -1);
                    return;
                }

                //进入匹配房间
                MatchRoom room = MatchCache.Instance.Enter(uid, clientPeer);
                Console.WriteLine("玩家:" + uid + "进入匹配房间:" + room.ID);
                //向房间内的除了自己外的人广播有玩家进入
                UserModel userModel = UserCache.Instance.GetModelByClientPeer(clientPeer);
                UserDto userDto = new UserDto(userModel.Id, userModel.Name, userModel.Been, userModel.WinCount, userModel.LoseCount, userModel.RunCount, userModel.Lv, userModel.Exp);
                //给其他发送自己的用户数据模型
                room.Broadcast(OpCode.MATCH, MatchCode.ENTER_BOD, userDto, clientPeer);

                //给玩家传回房间数据模型
                MatchRoomDto roomDto;
                makeRoomdto(room, out roomDto);
                clientPeer.StartSend(OpCode.MATCH, MatchCode.ENTER_SRES, roomDto);
            }); 
        }

        /// <summary>
        /// 创建房间数据传输对象
        /// </summary>
        /// <param name="matchRoom"></param>
        /// <param name="roomDto"></param>
        private void makeRoomdto(MatchRoom matchRoom , out MatchRoomDto roomDto)
        {
            roomDto = new MatchRoomDto();
            foreach (var uid in matchRoom.UidClientpeerDic.Keys)
            {
                UserModel userModel = UserCache.Instance.GetModelByClientPeer(matchRoom.UidClientpeerDic[uid]);
                UserDto userDto = new UserDto(userModel.Id,userModel.Name, userModel.Been, userModel.WinCount, userModel.LoseCount, userModel.RunCount, userModel.Lv, userModel.Exp);
                //roomDto.UidUdtoDic.Add(uid, userDto);
                roomDto.Add(uid, userDto);
            }
            roomDto.ReadyUidlist = matchRoom.ReadyUidlist;
        }

        /// <summary>
        /// 离开匹配队列
        /// </summary>
        /// <param name="clientPeer"></param>
        private void leave(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(() =>
            {
                if (!UserCache.Instance.IsOnline(clientPeer))
                {
                    return;
                }

                int uid = UserCache.Instance.GetId(clientPeer);
                //用户不在匹配队列
                if (!MatchCache.Instance.IsMatching(uid))
                {
                    return;
                }

                MatchRoom room =  MatchCache.Instance.Leave(uid);
                Console.WriteLine("玩家:" + uid + "离开匹配房间:" + room.ID);
                //向房间内所有人广播有玩家离开
                room.Broadcast(OpCode.MATCH, MatchCode.LEAVE_BOD, uid);
            });
        }

        private void ready(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }

                    int uid = UserCache.Instance.GetId(clientPeer);

                    if (!MatchCache.Instance.IsMatching(uid))
                    {
                        return;
                    }

                    MatchRoom room = MatchCache.Instance.GetRoom(uid);
                    room.Ready(uid);
                    //通知房间内除了自己以外的人，自己准备了
                    room.Broadcast(OpCode.MATCH, MatchCode.READY_BOD, uid, clientPeer);
                    Console.WriteLine("玩家:" + uid + "在匹配房间:" + room.ID + "准备了");
                    
                    //如果所有人都准备了则开始游戏
                    if (room.IsAllReady())
                    {
                        FightHandler.Instance.ToString();

                        if (StartGameEvent != null)
                        {
                            StartGameEvent(room.ReadyUidlist);
                        }

                        //开始游戏场景
                        room.Broadcast(OpCode.MATCH, MatchCode.START_GAME_BOD, "0");
                        //广播消息通知房间内所有人开始游戏
                        MatchCache.Instance.Destory(room);//销毁匹配房间重用
                        return;
                    }             
                }
                );
        }
    }
}
