using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 穿山甲
    /// </summary>
    public class OrcPangolin:ArmyCardBase
    {
        public OrcPangolin()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Pangolin;

            Race = RaceType.ORC;
            Class = ArmyClassType.HighClass;
            Damage = 1;
            MaxHp = 3;
            Hp = 3;
            Speed = 1;
            AttackRangeType = MapAttackType.Four_lattice;
            Position = null;
        }

        

        public void Skill()
        {
            //TODO 反击背刺
        }
    }
}
