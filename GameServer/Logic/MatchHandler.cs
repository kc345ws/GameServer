﻿using System;
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
using GameServer.Cache.Fight;

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

        /// <summary>
        /// 进入匹配队列
        /// </summary>
        /// <param name="clientPeer"></param>
        private void enter(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(() =>
            {
                if (!UserCache.Instance.IsOnline(clientPeer))
                {
                    //如果用户不在线
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

                UserDto userDto = new UserDto(userModel.Id, userModel.Name, userModel.Money, userModel.WinCount, userModel.LoseCount, userModel.RunCount, userModel.Lv, userModel.Exp);
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
                UserDto userDto = new UserDto(userModel.Id,userModel.Name, userModel.Money, userModel.WinCount, userModel.LoseCount, userModel.RunCount, userModel.Lv, userModel.Exp);
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

                UserModel userModel = UserCache.Instance.GetModelByClientPeer(clientPeer);
                MatchRoom room =  MatchCache.Instance.Leave(uid);
                Console.WriteLine("玩家:" + userModel.Name + "离开匹配房间:" + room.ID);
                //向房间内所有人广播有玩家离开
                room.Broadcast(OpCode.MATCH, MatchCode.LEAVE_BOD, uid);
            });
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="clientPeer"></param>
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
                    UserModel userModel = UserCache.Instance.GetModelByClientPeer(clientPeer);

                    if (!MatchCache.Instance.IsMatching(uid))
                    {
                        return;
                    }

                    MatchRoom room = MatchCache.Instance.GetRoom(uid);
                    room.Ready(uid);
                    //通知房间内除了自己以外的人，自己准备了
                    room.Broadcast(OpCode.MATCH, MatchCode.READY_BOD, uid, clientPeer);
                    Console.WriteLine("玩家" + userModel.Name + "在匹配房间:" + room.ID + "准备了");
                    
                    //如果所有人都准备了则开始游戏
                    if (room.IsAllReady())
                    {
                        //创建FightHandler
                        FightHandler.Instance.ToString();

                        /*if (StartGameEvent != null)
                        {
                            StartGameEvent(room.ReadyUidlist);
                        }*/
                        //创建战斗房间
                        FightRoom fightRoom = FightRoomCache.Instance.Create(room.ReadyUidlist);
                   
                        //广播消息通知房间内所有人开始选择选择种族
                        room.Broadcast(OpCode.FIGHT, FightCode.SELECT_RACE_SBOD, "开始选择种族");

                        MatchCache.Instance.Destory(room);//销毁匹配房间重用
                        return;
                    }             
                }
                );
        }
    }
}
