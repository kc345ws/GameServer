using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
using GameServer.Cache.Fight;
using GameServer.Cache;
using Protocol.Code;
using Protocol.Dto.Fight;
using ChcServer.Util.Concurrent;
using GameServer.Model;
using Protocol.Constants;

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
            //MatchHandler.Instance.StartGameEvent += startGame;
        }

        public void OnDisConnect(ClientPeer clientPeer)
        {
            processLeave(clientPeer);
        }

        public void OnReceive(ClientPeer clientPeer, int subcode, object value)
        {
            switch (subcode)
            {
                /*case FightCode.GRAB_LANDLORD_CREQ:
                    grabLandlord(clientPeer, (bool)value);
                    //true 抢 false不抢
                    break;
                case FightCode.DEAL_CREQ:
                    processdeal(clientPeer, value as DealDto);
                    break;

                case FightCode.PASS_CREQ:
                    processPass(clientPeer);
                    break;

                case FightCode.PLAYER_LEAVE_CREQ:
                    processLeave(clientPeer);
                    break;*/

                case FightCode.SELECT_RACE_CREQ:
                    processSelectRice(clientPeer, (int)value);
                    break;

                case FightCode.MAP_SET_ARMY_CREQ:
                    processMapSetArmy(clientPeer,value as MapPointDto);
                    break;

                case FightCode.DEAL_CARD_CREQ:
                    processDealCard(clientPeer);
                    break;

                case FightCode.MAP_ARMY_MOVE_CREQ:
                    ProcessArmyMove(clientPeer,value as MapMoveDto);
                    break;
            }
        }

        /// <summary>
        /// 处理兵种移动请求
        /// </summary>
        private void ProcessArmyMove(ClientPeer clientPeer,MapMoveDto mapMoveDto)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }

                    int uid = UserCache.Instance.GetId(clientPeer);
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);

                    //向房间内其他人传送移动信息
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.MAP_ARMY_MOVE_SBOD, mapMoveDto, clientPeer);
                }
                );
        }

        /// <summary>
        /// 处理出牌请求
        /// </summary>
        /// <param name="clientPeer"></param>
        private void processDealCard(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }

                    int uid = UserCache.Instance.GetId(clientPeer);
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);

                    //在其他玩家客户端移除手牌
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_CARD_SBOD, 1, clientPeer);
                }
                );
        }

        /// <summary>
        /// 处理地图放置兵种请求
        /// </summary>
        /// <param name="mapPointDto"></param>
        private void processMapSetArmy(ClientPeer clientPeer,MapPointDto mapPointDto)
        {
            
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }

                    int uid = UserCache.Instance.GetId(clientPeer);
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);

                    //向房间内其他人发送兵种放置消息
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.MAP_SET_ARMY_SBOD, mapPointDto, clientPeer);

                    //
                }
                );
        }
        

        /// <summary>
        /// 处理种族选择请求
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="race"></param>
        private void processSelectRice(ClientPeer clientPeer , int race)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    int uid = UserCache.Instance.GetId(clientPeer);
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                    switch (race)
                    {
                        case RaceType.ORC:
                            fightRoom.UidRaceidDic.Add(uid, RaceType.ORC);
                            break;
                    }

                    if(fightRoom.UidRaceidDic.Count >= fightRoom.playerDtos.Count)
                    {
                        //如果房间内的所有人都选择完了种族
                        startGame(fightRoom);
                    }
                }
                );
        }

        private void startGame(FightRoom fightRoom)//List<int> uidList)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    //开始游戏广播
                    fightRoom.Broadcast(OpCode.MATCH, MatchCode.START_GAME_BOD, "0");

                    //创建牌库
                    fightRoom.CreateCardLibrary();
                    //初始化玩家手牌(发牌)
                    fightRoom.InitPlayerCards();


                    //对洗牌后的手牌进行整理
                    //fightRoom.Sort();

                    foreach (var item in fightRoom.playerDtos)
                    {
                        //给每个客户端发送自己的手牌信息
                        List<CardDto> cardList = fightRoom.GetUserCard(item.UserID);
                        ClientPeer clientPeer = UserCache.Instance.GetClientPeer(item.UserID);
                        clientPeer.StartSend(OpCode.FIGHT, FightCode.GET_CARD_SRES, cardList);
                    }

                    //叫地主
                    //PlayerDto playerDto = fightRoom.GetFirstPlayer();//由第一个进入房间的首先叫地主
                    //fightRoom.Broadcast(OpCode.FIGHT, FightCode.GRAB_LANDLORD_SBOD, playerDto.UserID);
                    //轮到第一个玩家叫地主
                    //fightRoom.Broadcast(OpCode.FIGHT, FightCode.TURN_LANDLORD_SBOD, playerDto.UserID);*/
                }
                );

        }

        #region 其他代码
        private void processLeave(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }
                    int uid = UserCache.Instance.GetId(clientPeer);

                    if (!FightRoomCache.Instance.IsJoinFight(uid))
                    {
                        //如果没有进入战斗房间
                        return;
                    }

                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                    PlayerDto playerDto = fightRoom.GetPlayerDto(uid);

                    fightRoom.LeavePlayerDtos.Add(playerDto);
                    fightRoom.Leave(clientPeer);
                    fightRoom.UidRaceidDic.Remove(uid);

                    if (fightRoom.LeavePlayerDtos.Count >= 2)
                    {
                        //所有玩家都离开了
                        FightRoomCache.Instance.Destroy(fightRoom.ID);
                    }
                }
                );
        }

        private void processPass(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }
                    int uid = UserCache.Instance.GetId(clientPeer);

                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                    PlayerDto playerDto = fightRoom.GetPlayerDto(uid);

                    if (fightRoom.roundModle.BiggestUid == playerDto.UserID)
                    {
                        //clientPeer.StartSend(OpCode.FIGHT, FightCode.PASS_SRES, false);
                        //最大出牌者不能不出
                        return;
                    }
                    else
                    {
                        turn(fightRoom);//轮换出牌
                        //clientPeer.StartSend(OpCode.FIGHT, FightCode.PASS_SRES, true);
                    }
                }
                );
        }

        private void processdeal(ClientPeer client, DealDto dealDto)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(client))
                    {
                        return;
                    }
                    int uid = UserCache.Instance.GetId(client);

                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                    PlayerDto playerDto = fightRoom.GetPlayerDto(uid);
                    if (fightRoom.LeavePlayerDtos.Contains(playerDto))
                    {
                        //如果玩家已经离开了
                        //轮换出牌
                        turn(fightRoom);
                        return;
                    }

                    //发牌成功则移除手牌
                    //bool candeal = fightRoom.IsDealCard(dealDto.Type, dealDto.Weight, dealDto.Length, uid, dealDto.SelectCards);
                    /*if (!candeal)
                    {
                        client.StartSend(OpCode.FIGHT, FightCode.DEAL_SRES, false);
                        //向客户端回复不能出牌
                        return;
                    }*/
                    else
                    {
                        //向客户端发送发牌成功
                        //client.StartSend(OpCode.FIGHT, FightCode.DEAL_SRES, true);



                        List<CardDto> cardlist = fightRoom.GetUserCard(uid);

                        //玩家剩余手牌
                        dealDto.remainCards = cardlist;
                        //向客户端广播出牌信息
                        //fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_SBOD, dealDto);
                        if (cardlist.Count == 0)
                        {
                            //若手牌发完则游戏结束
                            gameover(fightRoom, uid);
                            //fightRoom.Broadcast(OpCode.FIGHT, FightCode.GAME_OVER_SBOD, true);
                        }
                        else
                        {
                            //轮换出牌
                            turn(fightRoom);
                        }
                    }
                }
                );
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        private void turn(FightRoom fightRoom)
        {
            int nextuid = fightRoom.Turn();
            if (fightRoom.IsOffline(nextuid))
            {
                //如果下一个玩家离线
                //递归找到一个在线的玩家
                turn(fightRoom);
            }
            else
            {
                //通知房间内的房间下一个该出牌的玩家
                //fightRoom.Broadcast(OpCode.FIGHT, FightCode.TURN_DEAL_SBOD, nextuid);
            }
        }

        private void gameover(FightRoom fightRoom, int winuid)
        {
            PlayerDto playerDto = fightRoom.GetPlayerDto(winuid);
            int winIdentity = playerDto.Identity;
            List<PlayerDto> winList = fightRoom.GetSameIdentityPlayer(winIdentity);
            List<PlayerDto> loseList = fightRoom.GetDiffIdentityPlayer(winIdentity);
            //int winbeen = fightRoom.Multiple * 100;
            //int losebeen = fightRoom.Multiple * 100 * 2;
            //int runbeen = fightRoom.Multiple * 100  * 3;

            //给胜利玩家增加
            for (int i = 0; i < winList.Count; i++)
            {
                ClientPeer client = UserCache.Instance.GetClientPeer(winList[i].UserID);
                UserModel um = UserCache.Instance.GetModelByClientPeer(client);

                if (!fightRoom.LeavePlayerDtos.Contains(winList[i]))
                {
                    //如果玩家没有中途离开
                    //um.Money += winbeen;
                    um.Exp += 20;
                    if (um.Exp >= 100)
                    {
                        um.Exp = 0;
                        um.Lv++;
                    }
                    um.WinCount++;
                    UserCache.Instance.Update(um);
                }
            }

            for (int i = 0; i < loseList.Count; i++)
            {
                ClientPeer client = UserCache.Instance.GetClientPeer(loseList[i].UserID);
                UserModel um = UserCache.Instance.GetModelByClientPeer(client);

                if (!fightRoom.LeavePlayerDtos.Contains(loseList[i]))
                {
                    //如果玩家没有中途离开
                    //um.Money -= losebeen;
                    um.Exp += 10;
                    if (um.Exp >= 100)
                    {
                        um.Exp = 0;
                        um.Lv++;
                    }
                    um.LoseCount++;
                    UserCache.Instance.Update(um);
                }
            }

            for (int i = 0; i < fightRoom.LeavePlayerDtos.Count; i++)
            {
                //PlayerDto runplayer = fightRoom.LeavePlayerDtos[i];
                ClientPeer client = UserCache.Instance.GetClientPeer(fightRoom.LeavePlayerDtos[i].UserID);
                UserModel um = UserCache.Instance.GetModelByClientPeer(client);
                //um.Money -= runbeen;
                um.LoseCount++;
                um.RunCount++;
                UserCache.Instance.Update(um);
            }

            //OverDto overDto = new OverDto(winIdentity, winList, winbeen);
            //fightRoom.Broadcast(OpCode.FIGHT, FightCode.GAME_OVER_SBOD, overDto);
        }

        /// <summary>
        /// 抢地主
        /// </summary>
        /// <param name="client"></param>
        /// <param name="active"></param>
        private void grabLandlord(ClientPeer client, bool active)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(client))
                    {
                        return;
                    }
                    int uid = UserCache.Instance.GetId(client);
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);

                    if (active)
                    {
                        //设置为地主
                        fightRoom.SetLandLord(uid);
                        //向每个客户端发送底牌信息以及谁抢了
                        //LandLordDto landLordDto = new LandLordDto(uid, fightRoom.TableCards);
                        //fightRoom.Broadcast(OpCode.FIGHT, FightCode.GRAB_LANDLORD_SBOD, landLordDto);

                        //通知房间内所有玩家，该玩家进行出牌
                        //fightRoom.Broadcast(OpCode.FIGHT, FightCode.TURN_DEAL_SBOD, uid);
                    }
                    else
                    {
                        //不抢
                        int nextuid = fightRoom.GetNextUid(uid);
                        //fightRoom.Broadcast(OpCode.FIGHT, FightCode.TURN_LANDLORD_SBOD, nextuid);
                        //下一个玩家继续抢
                    }
                }
                );
        }
        #endregion


        
    }
}
