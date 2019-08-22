using Protocol.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 卡牌数据传输对象
    /// </summary>
    /// 
    [Serializable]
    public class CardDto
    {
        public int ID;
        public int Type;//卡牌种类
        public int OtherType;//非指令卡种类
        public int Race;//兵种卡或非指令卡所属种族
        public int Class;//兵种卡阶级
        public int Name;//卡牌名称

        public bool CanFly;

        public CardDto() { }

        /// <summary>
        /// 指令卡
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CardDto(int id ,int name) {
            ID = id;
            Type = CardType.ORDERCARD;
            Name = name;
            OtherType = OtherCardType.NONE;
            Race = RaceType.NONE;
            Class = ArmyClassType.NONE;
        }

        /// <summary>
        /// 兵种卡
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="race"></param>
        public CardDto(int id , int race , int name ,bool canfly ,int Class)
        {
            ID = id;
            Type = CardType.ARMYCARD;
            Name = name;
            Race = race;
            OtherType = OtherCardType.NONE;
            CanFly = canfly;
            this.Class = Class;
        }

        /// <summary>
        /// 非指令卡
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="race"></param>
        /// <param name="othertype"></param>
        public CardDto(int id , int othertype, int race , int name)
        {
            ID = id;
            Type = CardType.OTHERCARD;
            Name = name;
            Race = race;
            OtherType = othertype;
            Class = ArmyClassType.NONE;
        }
    }
}
