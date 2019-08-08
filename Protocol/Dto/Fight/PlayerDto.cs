using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class PlayerDto
    {
        public int UserID { get; private set; }

        public int Identity { get; private set; }

        public List<CardDto> cardDtos { get; private set; }

        public PlayerDto(int id)
        {
            UserID = id;
            Identity = -1;
            cardDtos = new List<CardDto>();
        }

        public void AddCard(CardDto card)
        {
            cardDtos.Add(card);
        }

        public void RemoveCard(CardDto card)
        {
            cardDtos.Remove(card);
        }

        public bool HasCard()
        {
            return cardDtos.Count != 0;
        }

        public int GetCardCount()
        {
            return cardDtos.Count;
        }
    }
}
