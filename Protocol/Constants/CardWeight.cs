using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 卡牌权值
    /// </summary>
    public class CardWeight
    {
        public const int THREE = 3;
        public const int FOUR = 4;
        public const int FIVE = 5;
        public const int SIX = 6;
        public const int SEVEN = 7;
        public const int EIGHT = 8;
        public const int NENE = 9;
        public const int TEN = 10;

        public const int JACK = 11;//侍从
        public const int QUEEN = 12;//王后
        public const int KING = 13;//国王

        public const int A = 14;
        public const int TWO = 15;

        public const int SMALLJOKER = 16;
        public const int BIGJOKER = 17;

        public static string GetName(int weight)
        {
            switch (weight)
            {
                case 3:
                    return "Three";

                case 4:
                    return "Four";

                case 5:
                    return "Five";

                case 6:
                    return "Six";

                case 7:
                    return "Seven";

                case 8:
                    return "Eight";

                case 9:
                    return "Nine";

                case 10:
                    return "Ten";

                case 11:
                    return "Jack";

                case 12:
                    return "Queen";

                case 13:
                    return "King";

                case 14:
                    return "One";

                case 15:
                    return "Two";

                case 16:
                    return "SJoker";

                case 17:
                    return "LJoker";
            }
            return "";
        }
    }
}
