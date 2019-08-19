using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Dto.Fight;
using Protocol.Constants;
using Protocol.Constants.Orc;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 游戏的牌库
    /// </summary>
    public class CardLibrary
    {
        //public Queue<CardDto> cardDtos = new Queue<CardDto>();

        public Queue<IArmyCardBase>[] armyCards = new Queue<IArmyCardBase>[2];

        private int CardId = 0;

        /*public CardLibrary()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }*/

        public CardLibrary(Dictionary<int, int> UidRaceidDic)
        {
            Create(UidRaceidDic);
            shuffle();
        }

        /*public void Init()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }*/

        private void Create(Dictionary<int, int> UidRaceidDic)
        {
            int race = -1;         
            for(int playerindex = 0; playerindex < UidRaceidDic.Count; playerindex++)
            {
                race = UidRaceidDic[playerindex];

                switch (race)
                {
                    case RaceType.ORC:
                        //兽族
                        CreateOrc(playerindex);
                        
                        break;
                }
            }
        }

        /// <summary>
        /// 创建兽族牌组
        /// </summary>
        private void CreateOrc(int playerIndex)
        {
            Queue<IArmyCardBase> armycardQueue = armyCards[playerIndex];
            Random random = new Random();
            //17张兵种牌
            int ordinaryCount = random.Next(7, 12);//普通兵种7-11张
            int middleCount = random.Next(0, 6);//中级0-5           
            int HeroCount = 1;
            //剩余为髙阶
            int highCount = 17 - ordinaryCount - middleCount - HeroCount;

            int infantryCount = random.Next(ordinaryCount);//兽族步兵
            int eagleCount = ordinaryCount - infantryCount;//鹰骑士
            int blackRatsCount = random.Next(middleCount);//黑鼠爆破手
            int FrogCount = (middleCount - blackRatsCount)/2;//巨口蛙
            int ForestCount = middleCount - FrogCount - blackRatsCount;//射手
            int PangolinCount = random.Next(highCount);//穿山甲
            int RavenShamanCount = highCount - PangolinCount;//乌鸦萨满

            for(int i = 0; i < HeroCount; i++)
            {
                OrcHero orcHero = new OrcHero();
                armycardQueue.Enqueue(orcHero);
            }

            for (int i = 0; i < infantryCount; i++)
            {
                OrcInfantry orcInfantry = new OrcInfantry();
                armycardQueue.Enqueue(orcInfantry);
            }

            for(int i = 0; i < eagleCount; i++)
            {
                OrcEagleRiders orcEagleRiders = new OrcEagleRiders();
                armycardQueue.Enqueue(orcEagleRiders);
            }

            for(int i = 0; i < blackRatsCount; i++)
            {
                OrcBlackRatsBoomer orcBlackRatsBoomer = new OrcBlackRatsBoomer();
                armycardQueue.Enqueue(orcBlackRatsBoomer);
            }

            for(int i = 0; i < FrogCount; i++)
            {
                OrcGiantmouthedFrog orcGiantmouthedFrog = new OrcGiantmouthedFrog();
                armycardQueue.Enqueue(orcGiantmouthedFrog);
            }

            for(int i = 0; i < ForestCount; i++)
            {
                OrcForestShooter orcForestShooter = new OrcForestShooter();
                armycardQueue.Enqueue(orcForestShooter);
            }

            for(int i = 0; i < PangolinCount; i++)
            {
                OrcPangolin orcPangolin = new OrcPangolin();
                armycardQueue.Enqueue(orcPangolin);
            }

            for(int i = 0; i < RavenShamanCount; i++)
            {
                OrcRavenShaman orcRavenShaman = new OrcRavenShaman();
                armycardQueue.Enqueue(orcRavenShaman);
            }


            //指令卡25-28张
            int OrderCount = random.Next(25, 29);
        }

        private void Create()
        {
            /*
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
            cardDtos.Enqueue(bigJoker);*/
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
                        //Console.WriteLine(index);
                        hasindex = true;
                        break;
                    }
                }
            }


            /*foreach (var item in cardDtos)
            {
                int index = indexqueue.Dequeue();
                //shuffleCards.Insert(index, item);
                shuffleCards[index] = item;
            }

            cardDtos.Clear();

            foreach (var item in shuffleCards)
            {
                cardDtos.Enqueue(item);
            }*/
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        /*public CardDto DispatchCard()
        {
            //return cardDtos.Dequeue();
        }*/

    }
}
