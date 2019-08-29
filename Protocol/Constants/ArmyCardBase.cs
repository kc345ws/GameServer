using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    public abstract class ArmyCardBase :CardBase
    {
        /// <summary>
        /// 所属种族
        /// </summary>
        public int Race;

        /// <summary>
        /// 兵种阶级
        /// </summary>
        public int Class;

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Damage;

        /// <summary>
        /// 最大血量
        /// </summary>
        public int MaxHp;

        /// <summary>
        /// 当前血量
        /// </summary>
        public int Hp;

        /// <summary>
        /// 移动速度
        /// </summary>
        public int Speed;

        /// <summary>
        /// 攻击范围类型
        /// </summary>
        public int AttackRangeType;

        /// <summary>
        /// 先在所处的位置
        /// </summary>
        public MapPoint Position;

        /// <summary>
        /// 是否飞行
        /// </summary>
        public bool CanFly;


        /// <summary>
        /// 兵种技能
        /// </summary>
        public void Skill() { }
    }
}
