using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 地主信息
    /// </summary>
    [Serializable]
    public class LandLordDto
    {
        public int UserId;//地主玩家的ID
        public List<CardDto> TableCardList;
        public LandLordDto() { }
        public LandLordDto(int uid , List<CardDto> cardList)
        {
            UserId = uid;
            TableCardList = cardList;
        }
    }
}
