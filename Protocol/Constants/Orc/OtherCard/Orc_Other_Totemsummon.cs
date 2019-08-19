using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc.OtherCard
{
    /// <summary>
    /// 召唤图腾
    /// </summary>
    public class Orc_Other_Totemsummon : IOtherCardBase
    {
        public Orc_Other_Totemsummon()
        {
            Type = CardType.OTHERCARD;
            OtherType = OtherCardType.MagicCard;
            Name = OrcOtherCardType.Totem_summon;
        }

        public int Type { get => Type; set => Type = value; }
        public int Name { get => Name; set => Name = value; }
        public int OtherType { get => OtherType; set => OtherType = value; }

        public void Effect()
        {
            throw new NotImplementedException();
            //TODO 
        }
    }
}
