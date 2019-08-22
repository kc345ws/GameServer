﻿using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 乌鸦萨满
    /// </summary>
    public class OrcRavenShaman:IArmyCardBase
    {
        public OrcRavenShaman()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Raven_Shaman;

            Race = RaceType.ORC;
            Class = ArmyClassType.HighClass;
            Damage = 1;
            MaxHp = 3;
            Hp = 3;
            Speed = 1;
            AttackRangeType = MapAttackType.Four_Angle;
            Position = null;
        }

        public int Race { get => Race; set => Race = value; }
        public int Class { get => Class; set => Class = value; }
        public int Damage { get => Damage; set => Damage = value; }
        public int MaxHp { get => MaxHp; set => MaxHp = value; }
        public int Hp { get => Hp; set => Hp = value; }
        public int Speed { get => Speed; set => Speed = value; }
        public int AttackRangeType { get => AttackRangeType; set => AttackRangeType = value; }
        public MapPoint Position { get => Position; set => Position = value; }
        public int Type { get => Type; set => Type = value; }
        public int Name { get => Name; set => Name = value; }

        public void Skill()
        {
            //TODO 治疗
        }
    }
}