using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 黑鼠爆破手
    /// </summary>
    public class OrcBlackRatsBoomer:ArmyCardBase
    {
        public OrcBlackRatsBoomer()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Black_Rats_Boomer;

            Race = RaceType.ORC;
            Class = ArmyClassType.MiddleClass;
            Damage = 0;
            MaxHp = 2;
            Hp = 2;
            Speed = 2;
            AttackRangeType = MapAttackType.NONE;
            Position = null;

            MoveRangeType = MapMoveType.Four_lattice;
            MoveType = ArmyMoveType.UNDERGROUND;
            CanSlantAttack = false;
        }

        

        public void Skill()
        {
            //TODO 爆破
        }
    }
}
