using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 指令卡种类
    /// </summary>
    public class OrderCardType
    {
        public const int NONE = 0;

        /// <summary>
        /// 攻击卡
        /// </summary>
        public const int ATTACK = 1;

        /// <summary>
        /// 闪避卡
        /// </summary>
        public const int DODGE = 2;

        /// <summary>
        /// 反击卡
        /// </summary>
        public const int BACKATTACK = 3;

        /// <summary>
        /// 修养卡
        /// </summary>
        public const int REST = 4;

        /// <summary>
        /// 洗牌卡
        /// </summary>
        public const int SHUFFLE = 5;

        /// <summary>
        /// 抽牌卡
        /// </summary>
        public const int TAKE = 6;
    }
}
