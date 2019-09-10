using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
using GameServer.DataBase;
using GameServer.Model;
using Protocol.Constants;
using Protocol.Dto.Fight;

namespace GameServer.Cache.Fight
{
    public class FightRoom
    {
        /// <summary>
        /// 战斗房间ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 战斗房间中的玩家列表
        /// </summary>
        public List<PlayerDto> playerDtos { get; set; }

        /// <summary>
        /// 中途离开的玩家，用于结算时对其进行惩罚
        /// </summary>
        public List<PlayerDto> LeavePlayerDtos { get; private set; }

        /// <summary>
        /// 牌库
        /// </summary>
        public CardLibrary cardLibrary { get; private set; }

        /// <summary>
        /// 玩家ID与种族ID的映射
        /// </summary>
        public Dictionary<int,int>UidRaceidDic { get; private set; }

        /*/// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> TableCards { get; private set; }*/

            /// <summary>
            /// 玩家兵种管理
            /// </summary>
        public List<ArmyCardBase>[] ArmyList { get; private set; }

        /// <summary>
        /// 回合管理类
        /// </summary>
        public RoundModle roundModle { get; private set; }

        public FightRoom(int id , List<int>uidList)
        {
            ID = id;

            playerDtos = new List<PlayerDto>();
            foreach (var item in uidList)
            {
                PlayerDto playerDto = new PlayerDto(item);
                playerDtos.Add(playerDto);
            }

            LeavePlayerDtos = new List<PlayerDto>();
            //cardLibrary = new CardLibrary(List<int>uidList);
            //TableCards = new List<CardDto>();
            roundModle = new RoundModle();
            UidRaceidDic = new Dictionary<int, int>();
            ArmyList = new List<ArmyCardBase>[2];
            for(int i = 0; i < 2; i++)
            {
                ArmyList[i] = new List<ArmyCardBase>();
            }
        }

        /// <summary>
        /// 创建战斗房间的牌库
        /// </summary>
        public void CreateCardLibrary()
        {
            cardLibrary = new CardLibrary(UidRaceidDic);
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        public int Turn()
        {
            int curruid = roundModle.CurrentUid;
            int nextuid = GetNextUid(curruid);

            roundModle.CurrentUid = nextuid;
            return nextuid;
        }

        /// <summary>
        /// 玩家是否掉线
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsOffline(int uid)
        {
            PlayerDto player = null;
            foreach (var item in playerDtos)
            {
                if(item.UserID == uid)
                {
                    player = item;
                    return LeavePlayerDtos.Contains(player);
                }
            }
            return LeavePlayerDtos.Contains(player);
        }

        /// <summary>
        /// 玩家中途离开
        /// </summary>
        /// <param name="uid"></param>
        public void Leave(ClientPeer clientPeer)
        {
            UserModel userModel = UserCache.Instance.GetModelByClientPeer(clientPeer);
            userModel.Money -= 100;
            userModel.RunCount++;
            userModel.LoseCount++;

            MysqlPeer.Instance.UpdateUserInfo(userModel);
        }

        /// <summary>
        /// 获取下一个应该出牌玩家的ID
        /// </summary>
        /// <param name="currentid"></param>
        /// <returns></returns>
        public int GetNextUid(int currentid)
        {
            for(int i = 0; i < playerDtos.Count; i++)
            {
                if(currentid == playerDtos[i].UserID)
                {
                    //如果该玩家是玩家列表的最后一个玩家
                    if(i == playerDtos.Count -1)
                    {
                        return playerDtos[0].UserID;
                    }
                    return playerDtos[i + 1].UserID;
                }
            }
            throw new Exception("没有下个玩家");
        }
     
        /// <summary>
        /// 获取玩家的当前手牌
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<CardDto> GetUserCard(int uid)
        {
            PlayerDto playerdto = null;
            foreach (var item in playerDtos)
            {
                if(item.UserID == uid)
                {
                    playerdto = item;
                }
            }
            return playerdto.PlayerCards;
        }

        /// <summary>
        /// 移除玩家的手牌
        /// </summary>
        /// <param name="uid">要移除手牌的玩家ID</param>
        /// <param name="cardlist">要移除手牌的集合</param>
        public void RemoveCard(int uid, CardDto removeCard)
        {
            List<CardDto> playercardlist = GetUserCard(uid);

            foreach (var item in playercardlist)
            {
               if(item.ID == removeCard.ID)
               {
                    playercardlist.Remove(item);
                    break;
               }
            }
        }

        /// <summary>
        /// 移除玩家手牌
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type">手牌类型</param>
        /// <param name="name">手牌名称</param>
        public void RemoveCard(int uid, int type ,int name)
        {
            List<CardDto> playercardlist = GetUserCard(uid);

            foreach (var item in playercardlist)
            {
                if (item.Type == type && item.Name == name)
                {
                    playercardlist.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 初始化玩家手牌(发牌)
        /// </summary>
        public void InitPlayerCards()
        {
            for(int i = 0; i < 2; i++)
            {
                int count = 0;
                int index = 0;
                List<int> removeIndexlist = new List<int>();//要删除牌的索引
                foreach (var item in cardLibrary.playercardDtos[i])
                {
                    //开局每人9张兵种卡
                    if (item.Type == CardType.ARMYCARD)
                    {
                        count++;

                        //增加手牌
                        playerDtos[i].AddCard(item);
                        removeIndexlist.Add(index);
                        //Cards.Remove(item);
                        if (count > 8)
                        {
                            break;
                        }
                    }
                    index++;
                }

                //从牌库中删除发的牌
                for(int j = 0; j < removeIndexlist.Count; j++)
                {
                    cardLibrary.playercardDtos[i].RemoveAt(index);
                }
                count = 0;
                index = 0;
                removeIndexlist.Clear();
            }


            //每人开局摸5张其他牌
            for (int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    //增加手牌
                    playerDtos[i].AddCard(cardLibrary.playercardDtos[i][j]);

                    //从牌库中删除发的牌
                    cardLibrary.playercardDtos[i].RemoveAt(j);
                }
            }
        }

        /// <summary>
        /// 给玩家每回合发牌
        /// </summary>
        /// <param name="uid"></param>
        public List<CardDto> DispathCard(int uid)
        {
            PlayerDto player = GetPlayerDto(uid);
            List<CardDto> cardList = new List<CardDto>();
            for (int i = 0; i < 2; i++)
            {
                CardDto cardDto = cardLibrary.DispatchCard(uid);
                
                if(cardDto == null)
                {
                    break;
                }
                player.AddCard(cardDto);
                cardList.Add(cardDto);
            }
            return cardList;
        }    

        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public PlayerDto GetPlayerDto(int uid)
        {
            //in关键字不会改变内容
            foreach (var item in playerDtos)
            {
                if(item.UserID == uid)
                {
                    return item;
                }
            }
            throw new Exception("没有这个玩家");
        }

        /// <summary>
        /// 第一个进入房间的人首先开始
        /// </summary>
        /// <returns></returns>
        public PlayerDto GetFirstPlayer()
        {
            return playerDtos[0];
        }

        /// <summary>
        /// 按升序排序手牌
        /// </summary>
        /// <param name="cardList"></param>
        /// <param name="asc">是否升序</param>
       /* private void SortCard(List<CardDto> cardList,bool asc =true)//asc升序 des降序
        {
            cardList.Sort(delegate (CardDto a, CardDto b)
            {
                if (asc)
                {
                    //return a.Weight.CompareTo(b.Weight);
                }
                else
                {
                    //return a.Weight.CompareTo(b.Weight) * -1;
                }
            });
        }*/
      
        public void Sort(bool asc = true)
        {
            //SortCard(playerDtos[0].cardDtos);
            //SortCard(playerDtos[1].cardDtos);
            //SortCard(playerDtos[2].cardDtos);
            //SortCard(TableCards);
        }

        /// <summary>
        /// 向房间内的所有人广播
        /// </summary>
        /// <param name="room"></param>
        public void Broadcast(int opcode, int subcode, object value, ClientPeer exclientPeer = null)
        {
            SocketMsg mgr = new SocketMsg(opcode, subcode, value);
            byte[] data = EncodeTool.EncodeSocketMgr(mgr);
            byte[] packet = EncodeTool.EncodeMessage(data);//构造数据包

            foreach (var player in playerDtos)
            {             
                ClientPeer clientPeer = UserCache.Instance.GetClientPeer(player.UserID);
                if (clientPeer == exclientPeer)
                {
                    continue;//不用给自己广播消息
                }
                clientPeer.StartSend(packet);
            }
        }

    }    
}
