﻿using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc.OtherCard
{
    /// <summary>
    /// 复原虹吸
    /// </summary>
    public class Orc_Other_Recoverysiphon : IOtherCardBase
    {

        public Orc_Other_Recoverysiphon()
        {
            Type = CardType.OTHERCARD;
            OtherType = OtherCardType.MagicCard;
            Name = OrcOtherCardType.Recovery_siphon;
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
