using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 巨口蛙
    /// </summary>
    public class OrcGiantmouthedFrog:ArmyCardBase
    {
        public OrcGiantmouthedFrog()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Giant_mouthed_Frog;

            Race = RaceType.ORC;
            Class = ArmyClassType.MiddleClass;
            Damage = 1;
            MaxHp = 2;
            Hp = 2;
            Speed = 1;
            AttackRangeType = MapAttackType.Three_Front;
            Position = null;

            MoveRangeType = MapMoveType.Four_lattice;
            MoveType = ArmyMoveType.LAND;
            CanSlantAttack = false;
        }

        

        /*public void Skill()
        {
            //TODO 吞噬
        }*/
    }
}
