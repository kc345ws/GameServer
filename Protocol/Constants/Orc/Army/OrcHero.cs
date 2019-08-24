﻿using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 兽族英雄
    /// </summary>
    public class OrcHero:ArmyCardBase
    {
        public OrcHero()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Raven_Shaman;

            Race = RaceType.ORC;
            Class = ArmyClassType.Hero;
            Damage = 1;
            MaxHp = 10;
            Hp = 10;
            Speed = 2;
            AttackRangeType = MapAttackType.All_Around;
            Position = null;
        }



        public void Skill()
        {
            //TODO 狂怒血脉
        }
    }
}
