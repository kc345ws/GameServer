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
                case FightCode.GRAB_LANDLORD_CREQ:
                    grabLandlord(clientPeer, (bool)value);
                    //true 抢 false不抢
                    break;
                case FightCode.DEAL_CREQ:
                    processdeal(clientPeer, value as DealDto);
                    break;
            }
        }

        private void processdeal(ClientPeer client,DealDto dealDto)
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
                    PlayerDto playerDto =  fightRoom.GetPlayerDto(uid);
                    if (fightRoom.LeavePlayerDtos.Contains(playerDto))
                    {
                        //如果玩家已经离开了
                        //TODO 轮换出牌
                        return;
                    }

                    //发牌成功则移除手牌
                    bool candeal = fightRoom.IsDealCard(dealDto.Type, dealDto.Weight, dealDto.Length, uid, dealDto.SelectCards);
                    if (!candeal)
                    {
                        client.StartSend(OpCode.FIGHT, FightCode.DEAL_SRES, false);
                        //向客户端回复不能出牌
                        return;
                    }
                    else
                    {
                        //向客户端发送发牌成功
                        client.StartSend(OpCode.FIGHT, FightCode.DEAL_SRES, true);
                        //向客户端广播出牌信息
                        fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_SBOD, dealDto);

                        List<CardDto> cardlist = fightRoom.GetUserCard(uid);
                        if(cardlist.Count == 0)
                        {
                            //TODO 若手牌发完则游戏结束
                            fightRoom.Broadcast(OpCode.FIGHT, FightCode.GAME_OVER_SBOD, true);
                        }
                        else
                        {
                            //TODO 轮换出牌
                        }
                    }
                }
                );
        }

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
                        LandLordDto landLordDto = new LandLordDto(uid, fightRoom.TableCards);
                        fightRoom.Broadcast(OpCode.FIGHT, FightCode.GRAB_LANDLORD_SBOD, landLordDto);
                    }
                    else
                    {
                        //不抢
                        int nextuid = fightRoom.GetNextUid(uid);
                        fightRoom.Broadcast(OpCode.FIGHT, FightCode.TURN_LANDLORD_SBOD, nextuid);
                        //下一个玩家继续抢
                    }
                }
                );          
        }

        private void startGame(List<int> uidList)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    //创建战斗房间 以及牌库等
                    FightRoom fightRoom = FightRoomCache.Instance.Create(uidList);
                    //初始化玩家手牌(发牌)
                    fightRoom.InitPlayerCards();
                    //对洗牌后的手牌进行整理
                    fightRoom.Sort();
                    foreach (var item in uidList)
                    {
                        //给每个客户端发送自己的手牌信息
                        List<CardDto> cardList = fightRoom.GetUserCard(item);
                        ClientPeer clientPeer = UserCache.Instance.GetClientPeer(item);
                        clientPeer.StartSend(OpCode.FIGHT, FightCode.GET_CARD_SRES, cardList);
                    }

                    //叫地主
                    PlayerDto playerDto = fightRoom.GetFirstPlayer();//由第一个进入房间的首先叫地主
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.GRAB_LANDLORD_SBOD, playerDto.UserID);
                }
                );
            

        }
    }
}
