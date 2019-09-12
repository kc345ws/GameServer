using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 兵种移动方式
    /// </summary>
    public class ArmyMoveType
    {
        /// <summary>
        /// 无法移动
        /// </summary>
        public const int NONE = 0;

        /// <summary>
        /// 陆地
        /// </summary>
        public const int LAND = 1;

        /// <summary>
        /// 飞行
        /// </summary>
        public const int SKY = 2;

        /// <summary>
        /// 地下
        /// </summary>
        public const ushort UNDERGROUND= 3;

        /// <summary>
        /// 寄生
        /// </summary>
        public const ushort PARASITIC = 4;
    }
}
