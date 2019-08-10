using Protocol.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    public class CardType
    {
        public const int NONE = 0;
        public const int SINGLE = 1;
        public const int DOUBLE = 2;

        /// <summary>
        /// 单顺
        /// </summary>
        public const int STRIGHT = 3;// 456789
        public const int DOUBLE_STRIGHT = 4;// 44 55 66 
        public const int THREE_STRIGHT = 5;// 444 555 666
        


        public const int THREE = 6; ///444
            
        public const int THREE_ONE = 7;///4445
        
        ///三代一对
        public const int THREE_DOUBLE = 8;

        public const int FOUR_DOUBLE = 9;//444423 444455
        public const int BOOM = 10;//4444
        public const int JOKER_BOOM = 11;//

        /// <summary>
        /// 飞机
        /// </summary>
        public const int THREE_STRIGHT_FLIGHT = 12;///44455567 4445556677



        public static int GetType(List<CardDto> cardlist)
        {
            int type = CardType.NONE;

            if (IsSingle(cardlist))
            {
                type = CardType.SINGLE;
            }
            else if (IsDouble(cardlist))
            {
                type = CardType.DOUBLE;
            }
            else if (IsThree(cardlist))
            {
                type = CardType.THREE;
            }
            else if (IsStright(cardlist))
            {
                type = CardType.STRIGHT;
            }
            else if (IsStrightDouble(cardlist))
            {
                type = CardType.DOUBLE_STRIGHT;
            }
            else if (IsStrightThree(cardlist))
            {
                type = CardType.THREE_STRIGHT;
            }else if (IsThreeOne(cardlist))
            {
                type = CardType.THREE_ONE;
            }
            else if (IsThreeDouble(cardlist))
            {
                type = CardType.THREE_DOUBLE;
            }
            else if (IsFourDouble(cardlist))
            {
                type = CardType.FOUR_DOUBLE;
            }
            else if (IsBoom(cardlist))
            {
                type = CardType.BOOM;
            }
            else if (IsJokerBoom(cardlist))
            {
                type = CardType.JOKER_BOOM;
            }
            else
            {
                type = CardType.THREE_STRIGHT_FLIGHT;
            }


            return type;
        }
        /// <summary>
        /// 是否为单牌
        /// </summary>
        /// <returns></returns>

        public static bool IsSingle(List<CardDto> cardlist)
        {
            if (cardlist.Count == 1)
            {
                return true;
            }

            return false;
        }

        public static  bool IsDouble(List<CardDto> cardlist)
        {
            if (cardlist.Count == 2 && cardlist[0].Weight == cardlist[1].Weight)
            {
                return true;
            }
            return false;
        }

        public static bool IsThree(List<CardDto> cardlist)
        {
            if (cardlist.Count == 3 && cardlist[0].Weight == cardlist[1].Weight && cardlist[1].Weight == cardlist[2].Weight)
            {
                return true;
            }
            return false;
        }

        public static bool IsThreeOne(List<CardDto> cardlist)
        {
            if (cardlist[0].Weight == cardlist[1].Weight && cardlist[1].Weight == cardlist[2].Weight)
            {
                if (cardlist[0].Weight != cardlist[3].Weight)
                {
                    return true;
                }
            }

            else if (cardlist[1].Weight == cardlist[2].Weight && cardlist[2].Weight == cardlist[3].Weight)
            {
                if (cardlist[0].Weight != cardlist[1].Weight)
                {
                    return true;
                }
            }
            return false;
        }

        public static  bool IsThreeDouble(List<CardDto> cardlist)
        {
            if (cardlist[0].Weight == cardlist[1].Weight && cardlist[1].Weight == cardlist[2].Weight)
            {
                if (cardlist[0].Weight != cardlist[3].Weight && cardlist[3].Weight == cardlist[4].Weight)
                {
                    return true;
                }
            }

            else if (cardlist[2].Weight == cardlist[3].Weight && cardlist[3].Weight == cardlist[4].Weight)
            {
                if (cardlist[0].Weight != cardlist[2].Weight && cardlist[0].Weight == cardlist[1].Weight)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsFourDouble(List<CardDto> cardlist)
        {
            if(cardlist.Count == 6)
            {
                if(cardlist[0].Weight == cardlist[1].Weight && cardlist[1].Weight == cardlist[2].Weight && cardlist[2].Weight == cardlist[3].Weight)
                {
                    return true;
                }
                else if(cardlist[2].Weight == cardlist[3].Weight && cardlist[3].Weight == cardlist[4].Weight && cardlist[4].Weight == cardlist[5].Weight)
                {
                    return true;
                }
            }         
            return false;
        }

        /// <summary>
        /// 是否为单顺
        /// </summary>
        /// <param name="cardlist"></param>
        /// <returns></returns>
        public static bool IsStright(List<CardDto> cardlist)
        {
            int temp = -1;
            if (cardlist.Count < 5 || cardlist.Count > 12)
            {
                return false;
            }

            for (int i = 0; i < cardlist.Count - 1; i++)
            {
                if (cardlist[i].Weight == CardWeight.A || cardlist[i].Weight == CardWeight.TWO
                    || cardlist[i + 1].Weight == CardWeight.A || cardlist[i + 1].Weight == CardWeight.TWO)
                {
                    return false;
                }

                temp = cardlist[i + 1].Weight - cardlist[i].Weight;
                if (temp != 1)
                {
                    return false;
                }
            }

            if (temp == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否为双顺
        /// </summary>
        /// <param name="cardlist"></param>
        /// <returns></returns>
        public static bool IsStrightDouble(List<CardDto> cardlist)
        {
            int temp = -1;
            if (cardlist.Count < 6 || cardlist.Count % 2 != 0)
            {
                return false;
            }

            for (int i = 0; i < cardlist.Count - 2; i += 2)
            {
                if (cardlist[i].Weight == CardWeight.A || cardlist[i].Weight == CardWeight.TWO
                    || cardlist[i + 2].Weight == CardWeight.A || cardlist[i + 2].Weight == CardWeight.TWO)
                {
                    return false;
                }

                temp = cardlist[i + 2].Weight - cardlist[i].Weight;
                if (temp != 1)
                {
                    return false;
                }
            }
            if (temp == 1)
            {
                return true;
            }

            return false;
        }

        public static bool IsStrightThree(List<CardDto> cardlist)
        {
            int temp = -1;
            if (cardlist.Count < 6 || cardlist.Count % 2 != 0)
            {
                return false;
            }

            for (int i = 0; i < cardlist.Count - 3; i += 3)
            {
                if (cardlist[i].Weight == CardWeight.A || cardlist[i].Weight == CardWeight.TWO
                    || cardlist[i + 3].Weight == CardWeight.A || cardlist[i + 3].Weight == CardWeight.TWO)
                {
                    return false;
                }

                temp = cardlist[i + 3].Weight - cardlist[i].Weight;
                if (temp != 1)
                {
                    return false;
                }
            }
            if (temp == 1)
            {
                return true;
            }

            return false;
        }

        public static bool IsJokerBoom(List<CardDto> cardlist)
        {
            if (cardlist[0].Weight == CardWeight.SMALLJOKER && cardlist[1].Weight == CardWeight.BIGJOKER)
            {
                return true;
            }
            else if (cardlist[0].Weight == CardWeight.BIGJOKER && cardlist[1].Weight == CardWeight.SMALLJOKER)
            {
                return true;
            }
            return false;
        }

        public static bool IsBoom(List<CardDto> cardlist)
        {
            if (cardlist.Count == 4)
            {
                if (cardlist[0].Weight == cardlist[1].Weight && cardlist[1].Weight == cardlist[2].Weight && cardlist[2].Weight == cardlist[3].Weight)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是飞机
        /// </summary>
        /// <param name="cardlist"></param>
        /// <returns></returns>
        public static bool IsFlight(List<CardDto> cardlist)
        {
            int strightstartindex = -1;//三顺起始索引
            int strightlength = 0;//三顺长度
            int otherlength = 0;//其他长度
            bool isdouble = false;//其他手牌是否是对子
            int temp = -1;

            //找到顺子第一张牌的索引
            for(int i = 0; i < cardlist.Count; i++)
            {
                if(cardlist[i].Weight == cardlist[i+1].Weight &&cardlist[i+1].Weight==cardlist[i+2].Weight)
                {
                    strightstartindex = i;
                    break;
                }
            }        

            for(int i = strightstartindex;i < cardlist.Count -3; i+=3)
            {
                if (cardlist[i].Weight == CardWeight.A || cardlist[i].Weight == CardWeight.TWO
                    || cardlist[i + 3].Weight == CardWeight.A || cardlist[i + 3].Weight == CardWeight.TWO)
                {
                    return false;
                }

                temp = cardlist[i + 3].Weight - cardlist[i].Weight;
                if(temp != 1)
                {
                    return false;
                }
                else
                {
                    strightlength++;
                }
            }

            //如果顺子起始不在第一张牌
            if (strightstartindex != 0)
            {
                for(int i = 0; i < strightstartindex; i++)
                {
                    if(cardlist[i].Weight == cardlist[i + 1].Weight)
                    {
                        isdouble = true;
                    }
                }
                if (isdouble)
                {
                    for(int i = 0; i < strightstartindex; i += 2)
                    {
                        otherlength++;
                    }
                }
                else
                {
                    for (int i = 0; i < strightstartindex; i ++)
                    {
                        otherlength++;
                    }
                }
            }
            else//如果顺子起始在第一张牌
            {
                for(int i = strightstartindex * 3; i < cardlist.Count; i++)
                {
                    if (cardlist[i].Weight == cardlist[i + 1].Weight)
                    {
                        isdouble = true;
                    }  
                }
                if (isdouble)
                {
                    for (int i = strightstartindex * 3; i < cardlist.Count; i += 2)
                    {
                        otherlength++;
                    }
                }
                else
                {
                    for (int i = strightstartindex * 3; i < cardlist.Count; i++)
                    {
                        otherlength++;
                    }
                }
            }

                return false;
        }
    }
}
