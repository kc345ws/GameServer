using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc.OtherCard
{
    /// <summary>
    /// 地缚网
    /// </summary>
    public class Orc_Other_Groundfetternet : IOtherCardBase
    {
        public Orc_Other_Groundfetternet()
        {
            Type = CardType.OTHERCARD;
            OtherType = OtherCardType.TrapCard;
            Name = OrcOtherCardType.Ground_fetter_net;
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
