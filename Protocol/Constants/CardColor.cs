using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 牌的花色
    /// </summary>
    public class CardColor
    {
        /// <summary>
        /// 其他牌大小王等
        /// </summary>
        public const int NONE = 0;

        /// <summary>
        /// 黑桃
        /// </summary>
        public const int SPADE = 1;


        /// <summary>
        /// 梅花
        /// </summary>
        public const int HEART = 2;

        /// <summary>
        /// 梅花
        /// </summary>
        public const int CLUB = 3;

        /// <summary>
        /// 方片
        /// </summary>
        public const int DIAMOND = 4;

        public static string GetName(int color)
        {
            switch (color)
            {
                case 0:
                    return "";

                case 1:
                    return "Spade";

                case 2:
                    return "Heart";

                case 3:
                    return "Club";

                case 4:
                    return "Square";
            }
            return "";
        }

        

        

    }
}
