using Protocol.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 玩家出牌的数据传输对象
    /// </summary>
    /// 
    /*[Serializable]
    public class DealDto
    {
        /// <summary>
        /// 谁出的牌
        /// </summary>
        public int UserID;

        /// <summary>
        /// 用户选中想要出的牌
        /// </summary>
        public List<CardDto> SelectCards;

        public List<CardDto> remainCards;//剩余手牌

        public int Length;
        public int Weight;
        public int Type;
        /// <summary>
        /// 是否合法
        /// </summary>
        public bool isRegular;

        public DealDto()
        {

        }

        public DealDto(int uid , List<CardDto> cardlist)
        {
            UserID = uid;
            SelectCards = cardlist;
            //Type = CardType.GetType(cardlist);
            //Weight = CardWeight.GetWeight(cardlist, Type);      
            Length = cardlist.Count;

            //类型不为空则合法
            if (Type != CardType.NONE)
            {
                isRegular = true;
            }
            else
            {
                isRegular = false;
            }
        }
    }*/
}
