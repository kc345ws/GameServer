using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    public abstract class ArmyCardBase : CardBase
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
        /// 移动范围类型
        /// </summary>
        public int MoveRangeType;

        /// <summary>
        /// 技能范围类型
        /// </summary>
        public int SkillRangeType;

        /// <summary>
        /// 当前所处的位置
        /// </summary>
        public MapPoint Position;

        /// <summary>
        /// 是否飞行
        /// </summary>
        //public bool CanFly;

        /// <summary>
        /// 移动方式
        /// </summary>
        public ushort MoveType;

        /// <summary>
        /// 是否是我的
        /// </summary>
        public bool IsMine = true;

        /// <summary>
        /// 是否可以斜射
        /// </summary>
        public bool CanSlantAttack = false;

        /// <summary>
        /// 兵种技能
        /// </summary>
        //public void Skill() { }

        
    }
}
