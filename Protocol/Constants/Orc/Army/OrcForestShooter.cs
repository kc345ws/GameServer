using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 巫林射手
    /// </summary>
    public class OrcForestShooter:ArmyCardBase
    {
        public OrcForestShooter()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Forest_Shooter;

            Race = RaceType.ORC;
            Class = ArmyClassType.MiddleClass;
            Damage = 1;
            MaxHp = 2;
            Hp = 2;
            Speed = 1;
            AttackRangeType = MapAttackType.All_Around;
            Position = null;

            MoveRangeType = MapMoveType.Four_lattice;
            MoveType = ArmyMoveType.LAND;
            CanSlantAttack = true;
        }

       
        public void Skill()
        {
            //TODO 对空斜走
        }
    }
}
