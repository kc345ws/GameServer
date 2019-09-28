using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 兽族步兵
    /// </summary>
    public class OrcInfantry:ArmyCardBase
    {
        public OrcInfantry()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Infantry;

            Race = RaceType.ORC;
            Class = ArmyClassType.Ordinary;         
            Damage = 1;
            MaxHp = 1;
            Hp = 1;
            Speed = 1;
            AttackRangeType = MapAttackType.Triple_lattice;
            Position = null;

            MoveRangeType = MapMoveType.Triple_lattice;
            MoveType = ArmyMoveType.LAND;
            CanSlantAttack = false;
        }

        

    }
}
