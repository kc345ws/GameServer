using Protocol.Constants.Map;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Orc
{
    /// <summary>
    /// 鹰骑士
    /// </summary>
    public class OrcEagleRiders:ArmyCardBase
    {
        public OrcEagleRiders()
        {
            Type = CardType.ARMYCARD;
            Name = OrcArmyCardType.Eagle_Riders;

            Race = RaceType.ORC;
            Class = ArmyClassType.Ordinary;
            Damage = 1;
            MaxHp = 1;
            Hp = 1;
            Speed = 1;
            AttackRangeType = MapAttackType.Four_lattice;
            Position = null;

            MoveRangeType = MapMoveType.Triple_lattice;
            MoveType = ArmyMoveType.SKY;
            CanSlantAttack = false;
        }    

        public void Skill()
        {
            throw new NotImplementedException();
            //TODO 飞行
        }
    }
}
