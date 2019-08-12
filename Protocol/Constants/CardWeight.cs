using Protocol.Dto.Fight;
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

        public static void SortCard(ref List<CardDto> cardList, bool asc = true)//asc升序 des降序
        {
            cardList.Sort(delegate (CardDto a, CardDto b)
            {
                if (asc)
                {
                    return a.Weight.CompareTo(b.Weight);
                }
                else
                {
                    return a.Weight.CompareTo(b.Weight) * -1;
                }
            });
        }

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

        /// <summary>
        /// 获取各牌型的权值
        /// </summary>
        /// <param name="cardlist"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetWeight(List<CardDto> cardlist , int type)
        {
            int totalweight = 0;

            if(type == CardType.JOKER_BOOM)
            {
                totalweight = 99999999;
            }
            else if(type == CardType.BOOM)
            {
                foreach (var item in cardlist)
                {
                    totalweight += item.Weight * 1000;
                }
            }
            //三带一
            else if(type == CardType.THREE || type == CardType.THREE_ONE || type == CardType.THREE_DOUBLE)
            {
                for(int i = 0; i < cardlist.Count; i++)
                {
                    //三带一只算三
                    if (cardlist[i].Weight == cardlist[i+1].Weight && cardlist[i].Weight == cardlist[i + 2].Weight)
                    {
                        totalweight = cardlist[i].Weight * 3;
                        break;
                    }
                }
            }
                //四带二
            else if(type == CardType.FOUR_DOUBLE)
            {
                for (int i = 0; i < cardlist.Count; i++)
                {
                    //四代二只算四
                    if (cardlist[i].Weight == cardlist[i + 1].Weight && cardlist[i].Weight == cardlist[i + 2].Weight && cardlist[i].Weight == cardlist[i + 3].Weight)
                    {
                        totalweight = cardlist[i].Weight * 4;
                        break;
                    }
                }
            }
            //飞机
            else if (type == CardType.FOUR_DOUBLE)
            {
                for (int i = 0; i < cardlist.Count; i++)
                {
                    if (cardlist[i].Weight == cardlist[i + 1].Weight && cardlist[i].Weight == cardlist[i + 2].Weight)
                    {
                        totalweight = cardlist[i].Weight * 3;
                        i = i + 2;
                    }
                }
            }

            else
            {
                for(int i = 0; i < cardlist.Count; i++)
                {
                    totalweight += cardlist[i].Weight;
                }
            }

            return totalweight;
        }
    }
}
/*
 5 、牌型 
火箭：即双王（大王和小王），最大的牌。 
炸弹：四张同数值牌（如四个 7 ）。 
单牌：单个牌（如红桃 5 ）。 
对牌：数值相同的两张牌（如梅花 4+ 方块 4 ）。 
三张牌：数值相同的三张牌（如三个 J ）。 
三带一：数值相同的三张牌 + 一张单牌或一对牌。例如： 333+6 或 444+99 
单顺：五张或更多的连续单牌（如： 45678 或 78910JQK ）。不包括 2 点和双王。 
双顺：三对或更多的连续对牌（如： 334455 、7788991010JJ ）。不包括 2 点和双王。 
三顺：二个或更多的连续三张牌（如： 333444 、 555666777888 ）。不包括 2 点和双王。 
飞机带翅膀：三顺＋同数量的单牌（或同数量的对牌）。 
如： 444555+79 或 333444555+7799JJ 
四带二：四张牌＋两手牌。（注意：四带二不是炸弹）。 
如： 5555 ＋ 3 ＋ 8 或 4444 ＋ 55 ＋ 77 。 

6 、牌型的大小 
火箭最大，可以打任意其他的牌。 
炸弹比火箭小，比其他牌大。都是炸弹时按牌的分值比大小。 
除火箭和炸弹外，其他牌必须要牌型相同且总张数相同才能比大小。 
单牌按分值比大小，依次是 大王 > 小王 >2>A>K>Q>J>10>9>8>7>6>5>4>3 ，不分花色。 
对牌、三张牌都按分值比大小。 
顺牌按最大的一张牌的分值来比大小。 
飞机带翅膀和四带二按其中的三顺和四张部分来比，带的牌不影响大小。 
     */
