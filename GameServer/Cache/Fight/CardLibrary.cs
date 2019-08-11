using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Dto.Fight;
using Protocol.Constants;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 游戏的牌库
    /// </summary>
    public class CardLibrary
    {
        public Queue<CardDto> cardDtos = new Queue<CardDto>();

        private int CardId = 0;

        public CardLibrary()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }

        public void Init()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }

        private void Create()
        {
            //从黑桃到方片
            for(int color = CardColor.SPADE; color <= CardColor.DIAMOND; color++)
            {
                //从3到2
                for(int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    CardDto cardDto = new CardDto(CardId, CardColor.GetName(color) + CardWeight.GetName(weight), color, weight);
                    CardId++;
                    cardDtos.Enqueue(cardDto);
                }
            }

            CardDto smallJocker = new CardDto(CardId, CardWeight.GetName(CardWeight.SMALLJOKER), CardColor.NONE, CardWeight.SMALLJOKER);
            CardId++;
            cardDtos.Enqueue(smallJocker);

            CardDto bigJoker = new CardDto(CardId, CardWeight.GetName(CardWeight.BIGJOKER), CardColor.NONE, CardWeight.BIGJOKER);
            CardId++;
            cardDtos.Enqueue(bigJoker);
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        private void shuffle()
        {
            //List<int> listint = Enumerable.Range(1, 10000000).OrderBy(x => Guid.NewGuid()).Take(100000).ToList();
            List<CardDto> shuffleCards = new List<CardDto>();
            Queue<int> indexqueue = new Queue<int>();
            bool hasindex = false;
            for (int i = 0; i < 54; i++)
            {
                shuffleCards.Add(new CardDto());
                while (true)
                {
                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    int index = random.Next(0, 54);
                    hasindex = indexqueue.Contains(index);
                    if (hasindex == false)
                    {
                        indexqueue.Enqueue(index);
                        Console.WriteLine(index);
                        hasindex = true;
                        break;
                    }
                }
            }


            foreach (var item in cardDtos)
            {
                int index = indexqueue.Dequeue();
                //shuffleCards.Insert(index, item);
                shuffleCards[index] = item;
            }

            cardDtos.Clear();

            foreach (var item in shuffleCards)
            {
                cardDtos.Enqueue(item);
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        public CardDto DispatchCard()
        {
            return cardDtos.Dequeue();
        }

    }
}
