using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    public interface IArmyCardBase :ICardBase
    {
        /// <summary>
        /// 所属种族
        /// </summary>
        int Race { get; set; }

        /// <summary>
        /// 兵种阶级
        /// </summary>
        int Class { get; set; }

        /// <summary>
        /// 攻击力
        /// </summary>
        int Damage { get; set; }

        /// <summary>
        /// 最大血量
        /// </summary>
        int MaxHp { get; set; }

        /// <summary>
        /// 当前血量
        /// </summary>
        int Hp { get; set; }

        /// <summary>
        /// 移动速度
        /// </summary>
        int Speed { get; set; }

        /// <summary>
        /// 攻击范围类型
        /// </summary>
        int AttackRangeType { get; set; }

        /// <summary>
        /// 先在所处的位置
        /// </summary>
        MapPoint Position { get; set; }
        

        /// <summary>
        /// 兵种技能
        /// </summary>
        void Skill();
    }
}
