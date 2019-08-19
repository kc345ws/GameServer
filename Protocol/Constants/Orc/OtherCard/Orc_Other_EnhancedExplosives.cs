using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc.OtherCard
{
    /// <summary>
    /// 强化炸药 
    /// </summary>
    public class Orc_Other_EnhancedExplosives : IOtherCardBase
    {
        public Orc_Other_EnhancedExplosives()
        {
            Type = CardType.OTHERCARD;
            OtherType = OtherCardType.EquipCard;
            Name = OrcOtherCardType.Enhanced_Explosives;
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
