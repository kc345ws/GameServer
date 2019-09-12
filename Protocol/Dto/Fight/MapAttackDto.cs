using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 地图攻击传输类
    /// </summary>
    /// 
    [Serializable]
    public class MapAttackDto
    {
        /// <summary>
        /// 攻击方所在地图点
        /// </summary>
        public MapPoint AttacklMapPoint;
        /// <summary>
        /// 防守方所在地图点
        /// </summary>
        public MapPoint DefenseMapPoint;

        /// <summary>
        /// 攻击方移动方式
        /// </summary>
        public int AttackMoveType;

        public int DefenseMoveType;

        public MapAttackDto() { }

        /*public MapAttackDto(MapPoint attacklMapPoint, MapPoint defenseMapPoint, bool attackCanFly , bool defenseCanFly)
        {
            AttacklMapPoint = attacklMapPoint;
            DefenseMapPoint = defenseMapPoint;
            AttackCanFly = attackCanFly;
            DefenseCanFly = defenseCanFly;
        }*/
        public MapAttackDto(MapPoint attacklMapPoint, MapPoint defenseMapPoint , int attackMovetype , int defenseMovetype)
        {
            AttacklMapPoint = attacklMapPoint;
            DefenseMapPoint = defenseMapPoint;
            AttackMoveType = attackMovetype;
            DefenseMoveType = defenseMovetype;
        }

        public void Change(MapPoint attacklMapPoint, MapPoint defenseMapPoint, int attackMovetype, int defenseMovetype)
        {
            AttacklMapPoint = attacklMapPoint;
            DefenseMapPoint = defenseMapPoint;
            AttackMoveType = attackMovetype;
            DefenseMoveType = defenseMovetype;
        }
    }
}
