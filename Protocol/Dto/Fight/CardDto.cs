using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 卡牌数据传输对象
    /// </summary>
    public class CardDto
    {
        public int ID;
        public string Name;//卡牌名称
        public int Color;//卡牌花色
        public int Weight;//卡牌大小

        public CardDto() { }

        public CardDto(int id , string name , int color , int weight)
        {
            ID = id;
            Name = name;
            Color = color;
            Weight = weight;
        }
    }
}
