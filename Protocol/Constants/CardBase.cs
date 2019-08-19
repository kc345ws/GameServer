using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 卡牌基类
    /// </summary>
    public interface ICardBase
    {
        /// <summary>
        /// 卡牌种类
        /// </summary>
        int Type { get; set; }

        /// <summary>
        /// 卡牌名称
        /// </summary>
        int Name { get; set; }

    }
}
