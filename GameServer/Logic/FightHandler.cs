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
using Protocol.Constants.Orc.OtherCard;
using GameServer.DataBase;

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
                    if (instance == null)
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

                case FightCode.SELECT_RACE_CREQ://选择种族
                    processSelectRice(clientPeer, (int)value);
                    break;

                case FightCode.MAP_SET_ARMY_CREQ://地图上放置单位
                    processMapSetArmy(clientPeer, value as MapPointDto);
                    break;

                case FightCode.DEAL_ARMYCARD_CREQ://使用兵种卡请求
                    processDealCard(clientPeer , value as CardDto);
                    break;

                case FightCode.MAP_ARMY_MOVE_CREQ://单位在地图上移动
                    processArmyMove(clientPeer, value as MapMoveDto);
                    break;

                case FightCode.ARMY_ATTACK_CREQ://单位攻击请求
                    processArmyAttack(clientPeer, value as MapAttackDto);
                    break;

                case FightCode.DEAL_DODGE_CREQ://出闪避请求
                    processDealDodge(clientPeer, (bool)value);
                    break;

                case FightCode.DEAL_BACKATTACK_CREQ://出反击请求
                    processDealBackAttack(clientPeer, (bool)value);
                    break;

                case FightCode.DEAL_REST_CREQ://出修养请求
                    processDealRestAttack(clientPeer, value as MapPointDto);
                    break;

                case FightCode.USE_OTHERCARD_CREQ://使用非指令卡
                    processOtherCard(clientPeer, value as CardDto);
                    break;

                case FightCode.DEAL_ATTACK_CREQ://出攻击卡
                    processAttackCard(clientPeer);
                    break;


                case FightCode.NEXT_TURN_CREQ://下一回合
                    processNextTurn(clientPeer);
                    break;

                case FightCode.GAME_OVER_CREQ:
                    processGameOver(clientPeer);
                    break;
            }

        }

        #region 战斗卡牌请求

        /// <summary>
        /// 处理出牌请求
        /// </summary>
        /// <param name="clientPeer"></param>
        private void processDealCard(ClientPeer clientPeer ,CardDto cardDto)
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

                    //在服务器上移除玩家兵种卡
                    fightRoom.RemoveCard(uid, cardDto);

                    //在其他玩家客户端移除手牌
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_ARMYCARD_SBOD, 1, clientPeer);
                }
                );
        }

        /// <summary>
        /// 处理攻击卡请求
        /// </summary>
        /// <param name="clientPeer"></param>
        private void processAttackCard(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(() =>
            {
                if (!UserCache.Instance.IsOnline(clientPeer))
                {
                    return;
                }

                int uid = UserCache.Instance.GetId(clientPeer);
                FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                
                //发送攻击广播
                fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_ATTACK_SBOD, "使用了攻击卡", clientPeer);

                //在服务器上移除玩家攻击手牌
                fightRoom.RemoveCard(uid, CardType.ORDERCARD, OrderCardType.ATTACK);
            });
        }

        /// <summary>
        /// 处理非指令卡
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="cardDto"></param>
        private void processOtherCard(ClientPeer clientPeer, CardDto cardDto)
        {
            SingleExecute.Instance.processSingle(() =>
            {
                if (!UserCache.Instance.IsOnline(clientPeer))
                {
                    return;
                }

                int uid = UserCache.Instance.GetId(clientPeer);
                FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                fightRoom.Broadcast(OpCode.FIGHT, FightCode.USE_OTHERCARD_SBOD, cardDto, clientPeer);

                //在服务器上移除玩家相应非指令卡
                fightRoom.RemoveCard(uid, cardDto);
            });

        }

        /// <summary>
        /// 处理修养请求
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="active"></param>
        private void processDealRestAttack(ClientPeer clientPeer, MapPointDto mapPointDto)
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
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_REST_SBOD, mapPointDto, clientPeer);
                    //向房间内其他人发送修养广播

                    //在服务器移除修养指令卡
                    fightRoom.RemoveCard(uid, CardType.ORDERCARD, OrderCardType.REST);
                }
                );
        }

        /// <summary>
        /// 处理反击请求
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="active"></param>
        private void processDealBackAttack(ClientPeer clientPeer, bool active)
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

                    //向房间内其他人发送闪避广播
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_BACKATTACK_SBOD, active, clientPeer);
                    


                    //在服务器上移除玩家反击手牌
                    fightRoom.RemoveCard(uid, CardType.ORDERCARD, OrderCardType.BACKATTACK);
                }
                );
        }

        /// <summary>
        /// 处理闪避请求
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="active"></param>
        private void processDealDodge(ClientPeer clientPeer, bool active)
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

                    //向房间内其他人发送闪避广播
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.DEAL_DODGE_SBOD, active, clientPeer);

                    //在服务器上移除玩家闪避手牌
                    fightRoom.RemoveCard(uid, CardType.ORDERCARD, OrderCardType.DODGE);
                }
                );
        }
        #endregion

        #region 战斗单位请求

        /// <summary>
        /// 处理地图放置兵种请求
        /// </summary>
        /// <param name="mapPointDto"></param>
        private void processMapSetArmy(ClientPeer clientPeer, MapPointDto mapPointDto)
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
        /// 处理兵种攻击请求
        /// </summary>
        private void processArmyAttack(ClientPeer clientPeer, MapAttackDto mapAttackDto)
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
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.ARMY_ATTACK_SBOD, mapAttackDto, clientPeer);
                    //向房间内其他人发送攻击请求

                }
                );
        }

        /// <summary>
        /// 处理兵种移动请求
        /// </summary>
        private void processArmyMove(ClientPeer clientPeer, MapMoveDto mapMoveDto)
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
        #endregion

        #region 其他代码

        private void processGameOver(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(() =>
            {
                if (!UserCache.Instance.IsOnline(clientPeer))
                {
                    return;
                }

                int uid = UserCache.Instance.GetId(clientPeer);
                //获得战斗房间
                FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);

                //玩家数据更新
                UserModel userModel = UserCache.Instance.GetModelByClientPeer(clientPeer);
                userModel.Exp += 20;
                userModel.Money += 100;
                if(userModel.Exp >= 100)
                {
                    userModel.Lv++;
                }
                MysqlPeer.Instance.UpdateUserInfo(userModel);

                //向其他人广播游戏结束
                fightRoom.Broadcast(OpCode.FIGHT, FightCode.GAME_OVER_SBOD, "游戏结束", clientPeer);

                //更新其他人的数据
                int otheruid = -1;
                foreach (var item in fightRoom.playerDtos)
                {
                    if(item.UserID != uid)
                    {
                        
                        otheruid = item.UserID;
                        break;
                    }
                }
                ClientPeer otherclientPeer = UserCache.Instance.GetClientPeer(otheruid);
                userModel = UserCache.Instance.GetModelByClientPeer(otherclientPeer);
                userModel.Exp += 5;
                userModel.Money += 30;
                if (userModel.Exp >= 100)
                {
                    userModel.Lv++;
                }
                MysqlPeer.Instance.UpdateUserInfo(userModel);

                //TODO 销毁战斗房间
            });
        }
        /// <summary>
        /// 处理下一回合请求
        /// </summary>
        /// <param name="clientPeer"></param>
        private void processNextTurn(ClientPeer clientPeer)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }

                    int uid = UserCache.Instance.GetId(clientPeer);
                    //获得战斗房间
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);

                    int nextuid = -1;//下一回合的玩家ID
                    foreach (var item in fightRoom.playerDtos)
                    {
                        if(item.UserID != uid)
                        {
                            nextuid = item.UserID;
                            break;
                        }
                    }
                    //下一回玩家的套接字连接对象
                    ClientPeer nextClientpeer = UserCache.Instance.GetClientPeer(nextuid);

                    //广播通知下一回合玩家
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.NEXT_TURN_SBOD, nextuid);
                    //给下一回合的玩家发牌
                    List<CardDto> cardlist = fightRoom.DispathCard(nextuid);
                    //给玩家发送发牌消息
                    //fightRoom.Broadcast(OpCode.FIGHT, FightCode.ADD_CARD_SRES, cardlist);
                    nextClientpeer.StartSend(OpCode.FIGHT, FightCode.ADD_CARD_SRES, cardlist);
                    //给房间内除了下一回合的玩家发送发牌消息
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.ADD_CARD_SBOD, cardlist.Count, nextClientpeer);
                }
                );
        }

        /// <summary>
        /// 处理种族选择请求
        /// </summary>
        /// <param name="clientPeer"></param>
        /// <param name="race"></param>
        private void processSelectRice(ClientPeer clientPeer, int race)
        {
            SingleExecute.Instance.processSingle(
                () =>
                {
                    if (!UserCache.Instance.IsOnline(clientPeer))
                    {
                        return;
                    }

                    int uid = UserCache.Instance.GetId(clientPeer);
                    //获得战斗房间
                    FightRoom fightRoom = FightRoomCache.Instance.GetRoomByUid(uid);
                    switch (race)
                    {
                        case RaceType.ORC://选择了兽族
                            fightRoom.UidRaceidDic.Add(uid, RaceType.ORC);
                            break;
                    }

                    if (fightRoom.UidRaceidDic.Count >= fightRoom.playerDtos.Count)
                    {
                        //如果房间内的所有人都选择完了种族
                        startGame(fightRoom);
                    }
                }
                );
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="fightRoom"></param>
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

                    foreach (var item in fightRoom.playerDtos)
                    {
                        //给每个客户端发送自己的手牌信息
                        List<CardDto> cardList = fightRoom.GetUserCard(item.UserID);
                        ClientPeer clientPeer = UserCache.Instance.GetClientPeer(item.UserID);
                        clientPeer.StartSend(OpCode.FIGHT, FightCode.GET_CARD_SRES, cardList);
                    }

                    //由第一个进入房间的玩家首先开始回合
                    PlayerDto firstPlayer = fightRoom.GetFirstPlayer();
                    fightRoom.Broadcast(OpCode.FIGHT, FightCode.NEXT_TURN_SBOD, firstPlayer.UserID);
                }
                );

        }

        /// <summary>
        /// 处理玩家离开
        /// </summary>
        /// <param name="clientPeer"></param>
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

        /// <summary>
        /// 玩家跳过
        /// </summary>
        /// <param name="clientPeer"></param>
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

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="fightRoom"></param>
        /// <param name="winuid"></param>
        private void gameover(FightRoom fightRoom, int winuid)
        {
            PlayerDto playerDto = fightRoom.GetPlayerDto(winuid);
            //int winIdentity = playerDto.Identity;
            //List<PlayerDto> winList = fightRoom.GetSameIdentityPlayer(winIdentity);
            //List<PlayerDto> loseList = fightRoom.GetDiffIdentityPlayer(winIdentity);
            //int winbeen = fightRoom.Multiple * 100;
            //int losebeen = fightRoom.Multiple * 100 * 2;
            //int runbeen = fightRoom.Multiple * 100  * 3;

            //给胜利玩家增加
            /*for (int i = 0; i < winList.Count; i++)
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
            //fightRoom.Broadcast(OpCode.FIGHT, FightCode.GAME_OVER_SBOD, overDto);*/
        }

        
        #endregion     
    }
}
