using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Dto.Fight;

namespace GameServer.Cache.Fight
{
    public class FightRoom
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 房间中的玩家
        /// </summary>
        public List<PlayerDto> playerDtos { get; private set; }

        /// <summary>
        /// 中途离开的玩家，用于结算时对其进行惩罚
        /// </summary>
        public List<PlayerDto> LeavePlayerDots { get; private set; }

        /// <summary>
        /// 牌库
        /// </summary>
        public CardLibrary cardLibrary { get; private set; }

        /// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> TableCards { get; private set; }

        public int Multiple;//倍数

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

            LeavePlayerDots = new List<PlayerDto>();
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

        public int GetNextUid(int currentid)
        {
            for(int i = 0; i < playerDtos.Count; i++)
            {
                if(currentid == playerDtos[i].UserID)
                {
                    if(i == playerDtos.Count -1)
                    {
                        return playerDtos[0].UserID;
                    }
                    return playerDtos[i + 1].UserID;
                }
            }
            throw new Exception("没有下个玩家");
        }
    }
}
