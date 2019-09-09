using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol.Constants;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class PlayerDto
    {
        public int UserID { get; private set; }

        //public int Identity { get; set; }


        //public List<CardDto> cardDtos { get; private set; }

        /// <summary>
        /// 玩家手牌
        /// </summary>
        public List<CardDto> PlayerCards { get; private set; }

        public PlayerDto(int id)
        {
            UserID = id;
            //Identity = PlayerIdentity.FRAMER;
            //cardDtos = new List<CardDto>();
            PlayerCards = new List<CardDto>();
        }

        /// <summary>
        /// 增加手牌
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(CardDto card)
        {
            //cardDtos.Add(card);
            PlayerCards.Add(card);
        }

        /// <summary>
        /// 移除手牌
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(CardDto card)
        {
            //cardDtos.Remove(card);
            PlayerCards.Remove(card);
        }

        /// <summary>
        /// 是否还有手牌
        /// </summary>
        /// <returns></returns>
        public bool HasCard()
        {
            //return cardDtos.Count != 0;
            return PlayerCards.Count != 0;
        }

        /// <summary>
        /// 获得手牌数量
        /// </summary>
        /// <returns></returns>
        public int GetCardCount()
        {
            //return cardDtos.Count;
            return PlayerCards.Count;
        }
    }
}
