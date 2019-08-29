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
        /// 攻击方是否为飞行单位
        /// </summary>
        public bool AttackCanFly;

        public bool DefenseCanFly;

        public MapAttackDto() { }

        public MapAttackDto(MapPoint attacklMapPoint, MapPoint defenseMapPoint, bool attackCanFly , bool defenseCanFly)
        {
            AttacklMapPoint = attacklMapPoint;
            DefenseMapPoint = defenseMapPoint;
            AttackCanFly = attackCanFly;
            DefenseCanFly = defenseCanFly;
        }

        public void Change(MapPoint attacklMapPoint, MapPoint defenseMapPoint, bool attackCanFly, bool defenseCanFly)
        {
            AttacklMapPoint = attacklMapPoint;
            DefenseMapPoint = defenseMapPoint;
            AttackCanFly = attackCanFly;
            DefenseCanFly = defenseCanFly;
        }
    }
}
