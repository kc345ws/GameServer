using Protocol.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 卡牌种类
    /// </summary>
    public class CardType
    {
        public const int NONE = 0;
        /// <summary>
        /// 兵种卡
        /// </summary>
        public const int ARMYCARD = 1;

        /// <summary>
        /// 指令卡
        /// </summary>
        public const int ORDERCARD = 2;

        /// <summary>
        /// 其他非指令卡
        /// </summary>
        public const int OTHERCARD = 3;      
    }
}
