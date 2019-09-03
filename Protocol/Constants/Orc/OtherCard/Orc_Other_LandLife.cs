using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc.OtherCard
{
    /// <summary>
    /// 生灵之地
    /// </summary>
    public class Orc_Other_LandLife : IOtherCardBase
    {

        public Orc_Other_LandLife()
        {
            Type = CardType.OTHERCARD;
            Name = OrcOtherCardType.LandLife;
            OtherType = OtherCardType.ManorCard;
            
        }
        //public int Type { get => Type; set => Type = value; }
        //public int Name { get => Name; set => Name = value; }
        //public int OtherType { get => OtherType; set => OtherType = value; }

        public void Effect()
        {
            throw new NotImplementedException();
            //TODO 
        }
    }
}
