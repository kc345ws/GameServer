using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc.OtherCard
{
    /// <summary>
    /// 闪电链
    /// </summary>
    public class Orc_Other_LightningChain : IOtherCardBase
    {
        public Orc_Other_LightningChain()
        {
            Type = CardType.OTHERCARD;
            OtherType = OtherCardType.MagicCard;
            Name = OrcOtherCardType.Lightning_Chain;
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
