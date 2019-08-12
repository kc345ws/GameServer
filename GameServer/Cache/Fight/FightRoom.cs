using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer;
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
        /// 战斗房间中的玩家聊表
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
        /// 底牌
        /// </summary>
        public List<CardDto> TableCards { get; private set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple;

        /// <summary>
        /// 回合管理类
        /// </summary>
        public RoundModle roundModle { get; private set; }

        public FightRoom(int id , List<int>uidList)
        {
            ID = id;
            Multiple = 1;

            playerDtos = new List<PlayerDto>();
            foreach (var item in uidList)
            {
                PlayerDto playerDto = new PlayerDto(item);
                playerDtos.Add(playerDto);
            }

            LeavePlayerDtos = new List<PlayerDto>();
            cardLibrary = new CardLibrary();
            TableCards = new List<CardDto>();
            roundModle = new RoundModle();
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
            userModel.Been -= Multiple * 100;
            userModel.RunCount++;
            userModel.LoseCount++;
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
        /// 判断该出牌者的出牌能否大于上一个最大出牌
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        /// <param name="length"></param>
        /// <param name="uid">出牌玩家的ID</param>
        /// <param name="cardlist">玩家想要出牌的列表</param>
        /// <returns></returns>
        public bool IsDealCard(int type, int weight, int length, int uid, List<CardDto> cardlist)
        {
            bool candeal = false;
            //如果牌型相同且权值大于上一个出牌
            if(type == roundModle.LastType && weight > roundModle.LastWeight)
            {
                //如果为顺子还要对长度进行限制
                if(type == CardType.STRIGHT || type == CardType.DOUBLE_STRIGHT || type == CardType.THREE_STRIGHT)
                {
                    if(length == roundModle.LastLength)
                    {
                        candeal = true;
                    }
                }
                else//其他牌型
                {
                    candeal = true;
                }
            }

            else if(type == CardType.BOOM && roundModle.LastType != CardType.JOKER_BOOM)
            {
                candeal = true;
            }

            else if(type == CardType.JOKER_BOOM)
            {
                candeal = true;
            }

            if (candeal)
            {
                //如果能出牌则移除玩家的手牌
                RemoveCard(uid, cardlist);

                //翻倍
                if(type == CardType.BOOM)
                {
                    Multiple *= 2;
                }else if(type == CardType.JOKER_BOOM)
                {
                    Multiple *= 4;
                }

                //改变回合的最大出牌信息
                roundModle.Change(uid, length, weight, type);
            }

            return candeal;
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
            return playerdto.cardDtos;
        }

        /// <summary>
        /// 移除玩家的手牌
        /// </summary>
        /// <param name="uid">要移除手牌的玩家ID</param>
        /// <param name="cardlist">要移除手牌的集合</param>
        public void RemoveCard(int uid, List<CardDto> cardlist)
        {
            List<CardDto> playercardlist = GetUserCard(uid);

            for(int i = 0; i < cardlist.Count; i++)
            {
                foreach (var item in playercardlist)
                {
                    if(item == cardlist[i])
                    {
                        playercardlist.Remove(item);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化玩家手牌(发牌)
        /// </summary>
        public void InitPlayerCards()
        {
            //每人17张牌
            for(int i = 0; i < 3; i++)
            {
                PlayerDto playerDto = playerDtos[i];
                for(int j = 0; j < 17; j++)
                {
                    CardDto cardDto = cardLibrary.DispatchCard();
                    playerDto.cardDtos.Add(cardDto);
                }
            }
            //设置底牌
            for(int i = 0; i < 3; i++)
            {
                TableCards.Add(cardLibrary.DispatchCard());
            }
        }

        /// <summary>
        /// 给地主发底牌
        /// </summary>
        /// <param name="uid"></param>
        public void DispatchTableCard(int uid)
        {
            PlayerDto player = null;
            foreach (var item in playerDtos)
            {
                if(item.UserID == uid)
                {
                    player = item;
                }
            }

            if(player !=null && player.Identity == PlayerIdentity.LANDLORD)
            {
                for(int i = 0; i < 3; i++)
                {
                    player.cardDtos.Add(TableCards[i]);
                }                
            }
            else
            {
                throw new Exception("设置底牌出错");
            }
        }

        /// <summary>
        /// 设置地主
        /// </summary>
        /// <param name="uid"></param>
        public void SetLandLord(int uid)
        {
            PlayerDto player = null;
            foreach (var item in playerDtos)
            {
                if (item.UserID == uid)
                {
                    player = item;
                }
            }

            if (player != null)
            {
                player.Identity = PlayerIdentity.LANDLORD;
                //发底牌
                DispatchTableCard(uid);
                //开始回合
                roundModle.Start(uid);
            }
            else
            {
                throw new Exception("设置身份出错");
            }
        }

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
        /// 获取相同身份的玩家数据模型,用来结算
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<PlayerDto> GetSameIdentityPlayer(int identity)
        {
            List<PlayerDto> players = new List<PlayerDto>();
            foreach (var item in playerDtos)
            {
                if(item.Identity == identity)
                {
                    players.Add(item);
                }
            }
            return players;
        }

        /// <summary>
        /// 获取不同身份的玩家信息
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<PlayerDto> GetDiffIdentityPlayer(int identity)
        {
            List<PlayerDto> players = new List<PlayerDto>();
            foreach (var item in playerDtos)
            {
                if (item.Identity != identity)
                {
                    players.Add(item);
                }
            }
            return players;
        }



        /// <summary>
        /// 第一个进入房间的人第一个叫地主
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
        /// <param name="asc"></param>
        private void SortCard(List<CardDto> cardList,bool asc =true)//asc升序 des降序
        {
            cardList.Sort(delegate (CardDto a, CardDto b)
            {
                if (asc)
                {
                    return a.Weight.CompareTo(b.Weight);
                }
                else
                {
                    return a.Weight.CompareTo(b.Weight) * -1;
                }
            });

           

        }

        
        public void Sort(bool asc = true)
        {
            SortCard(playerDtos[0].cardDtos);
            SortCard(playerDtos[1].cardDtos);
            SortCard(playerDtos[2].cardDtos);
            SortCard(TableCards);
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
