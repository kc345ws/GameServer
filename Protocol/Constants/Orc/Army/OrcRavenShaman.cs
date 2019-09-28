using Protocol.Constants.Map;
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
    public class OrcRavenShaman:ArmyCardBase
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

            MoveRangeType = MapMoveType.Four_lattice;
            MoveType = ArmyMoveType.LAND;
            CanSlantAttack = true;
        }

       

        /*public void Skill()
        {
            //TODO 治疗
        }*/
    }
}
