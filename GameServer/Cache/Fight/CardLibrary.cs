﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol.Dto.Fight;
using Protocol.Constants;
using Protocol.Constants.Orc;
using Protocol.Constants.OrderCard;
using Protocol.Constants.Orc.OtherCard;
using ChcServer.Util.Concurrent;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 游戏的牌库
    /// </summary>
    public class CardLibrary
    {
        //public Queue<CardDto> []playercardDtos = new Queue<CardDto>[2];
        /// <summary>
        /// 玩家手牌
        /// </summary>
        public List<CardDto>[] playercardDtos = new List<CardDto>[2];

        //public List<IArmyCardBase>[] ArmyCards = new List<IArmyCardBase>[2];
       // public List<IOrderCardBase>[] OrderCards = new List<IOrderCardBase>[2];
        //public List<IOtherCardBase>[] OtherCards = new List<IOtherCardBase>[2];

        //public List<ICardBase>[] playerCards = new List<ICardBase>[2];

        /// <summary>
        /// 玩家ID与牌库的映射
        /// </summary>
        public Dictionary<int, List<CardDto>> UidCardsDic = new Dictionary<int, List<CardDto>>();

        private ConcurrentInt CardId = new ConcurrentInt(-1);//卡牌ID初始为-1
        private Dictionary<int, int> uid_indexDict = new Dictionary<int, int>();//玩家ID与索引的映射

        /*public CardLibrary()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }*/

        public CardLibrary(Dictionary<int, int> UidRaceidDic)
        {
            //创建牌
            Create(UidRaceidDic);
            //洗牌
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
            Queue<int> uidQueue = new Queue<int>();//玩家ID队列

            foreach (var item in UidRaceidDic.Keys)
            {
                uidQueue.Enqueue(item);
            }

            for(int playerindex = 0; playerindex < UidRaceidDic.Count; playerindex++)
            {
                race = UidRaceidDic[playerindex];
               

                switch (race)
                {
                    case RaceType.ORC:
                        //兽族
                        int uid = uidQueue.Dequeue();
                        CreateOrc(playerindex);
                        UidCardsDic.Add(uid, playercardDtos[playerindex]);

                        uid_indexDict.Add(uid, playerindex);
                        break;
                }
            }
        }

        /// <summary>
        /// 创建兽族牌库
        /// </summary>
        /// <param name="playerIndex">玩家索引</param>
        private void CreateOrc(int playerIndex)
        {
            //List<IArmyCardBase> armycardList = ArmyCards[playerIndex];
            //List<IOrderCardBase> orderCardList = OrderCards[playerIndex];
            //List<IOtherCardBase> otherCardList = OtherCards[playerIndex];

            //List<ICardBase> Cards = playerCards[playerIndex];
            playercardDtos[playerIndex] = new List<CardDto>();
            List<CardDto> cardDtos = playercardDtos[playerIndex];

            Random random = new Random();
            //17张兵种牌
            /*int ordinaryCount = random.Next(7, 12);//普通兵种7-11张
            int middleCount = random.Next(0, 6);//中级0-5           
            int HeroCount = 1;//英雄一张*/
            int ordinaryCount = 7;
            int middleCount = 5;     
            int HeroCount = 1;//英雄一张
            //剩余为髙阶
            int highCount = 4;

            int infantryCount = 4;//兽族步兵
            int eagleCount = 3;//鹰骑士
            int blackRatsCount = 0;//黑鼠爆破手
            int FrogCount = 3;//巨口蛙
            int ForestCount = 2;//射手
            int PangolinCount = 2;//穿山甲
            int RavenShamanCount = 2;//乌鸦萨满

            for(int i = 0; i < HeroCount; i++)
            {
                //OrcHero orcHero = new OrcHero();
                //armycardList.Add(orcHero);
                //cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Hero,false,ArmyClassType.Hero));
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Hero,ArmyMoveType.LAND, ArmyClassType.Hero));
            }

            for (int i = 0; i < infantryCount; i++)
            {
                //OrcInfantry orcInfantry = new OrcInfantry();
                //armycardList.Add(orcInfantry);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Infantry,ArmyMoveType.LAND,ArmyClassType.Ordinary));
            }

            for(int i = 0; i < eagleCount; i++)
            {
                //OrcEagleRiders orcEagleRiders = new OrcEagleRiders();
                //armycardList.Add(orcEagleRiders);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Eagle_Riders, ArmyMoveType.SKY,ArmyClassType.Ordinary));
            }

            for(int i = 0; i < blackRatsCount; i++)
            {
                //OrcBlackRatsBoomer orcBlackRatsBoomer = new OrcBlackRatsBoomer();
                //armycardList.Add(orcBlackRatsBoomer);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Black_Rats_Boomer, ArmyMoveType.UNDERGROUND,ArmyClassType.MiddleClass));
            }

            for(int i = 0; i < FrogCount; i++)
            {
                //OrcGiantmouthedFrog orcGiantmouthedFrog = new OrcGiantmouthedFrog();
                //armycardList.Add(orcGiantmouthedFrog);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Giant_mouthed_Frog, ArmyMoveType.LAND, ArmyClassType.MiddleClass));
            }

            for(int i = 0; i < ForestCount; i++)
            {
                //OrcForestShooter orcForestShooter = new OrcForestShooter();
                //armycardList.Add(orcForestShooter);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Forest_Shooter,ArmyMoveType.LAND ,ArmyClassType.MiddleClass));
            }

            for(int i = 0; i < PangolinCount; i++)
            {
                //OrcPangolin orcPangolin = new OrcPangolin();
                //armycardList.Add(orcPangolin);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Pangolin, ArmyMoveType.LAND,ArmyClassType.HighClass));
            }

            for(int i = 0; i < RavenShamanCount; i++)
            {
                //OrcRavenShaman orcRavenShaman = new OrcRavenShaman();
                //armycardList.Add(orcRavenShaman);
                cardDtos.Add(new CardDto(CardId.Add_Get(), RaceType.ORC, OrcArmyCardType.Raven_Shaman, ArmyMoveType.LAND,ArmyClassType.HighClass));
            }


            //指令卡24-28张
            int OrderCount = 26;
            int AttackCount = 12;
            int DodgeCount = 6;//闪避卡6张
            int BackAttackCount = 4;//反击卡4张
            int RestCount = 4;//修养卡
            int ShuffleCount = 0;//洗牌0张张
            //int TakeCount = OrderCount - AttackCount - DodgeCount - BackAttackCount - RestCount - ShuffleCount;
            int TakeCount = 0;//抽卡0张

            //指令卡24-28张
            /*int OrderCount = 0;
            int AttackCount = 0;//攻击卡10
            int DodgeCount = 0;//闪避卡5张
            int BackAttackCount = 0;//反击卡3张
            int RestCount = OrderCount;//修养卡2张
            int ShuffleCount = 0;//洗牌2张
            int TakeCount = 0;//抽卡*/

            for (int i = 0; i < AttackCount; i ++)
            {
                //Order_Attack order_Attack = new Order_Attack();
                //orderCardList.Add(order_Attack);
                cardDtos.Add(new CardDto(CardId.Add_Get(),OrderCardType.ATTACK));
            }

            for(int i = 0; i < DodgeCount; i++)
            {
                //Order_Dodge order_Dodge = new Order_Dodge();
                //orderCardList.Add(order_Dodge);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OrderCardType.DODGE));
            }

            for(int i = 0; i < BackAttackCount; i++)
            {
                //Order_BackAttack order_BackAttack = new Order_BackAttack();
                //orderCardList.Add(order_BackAttack);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OrderCardType.BACKATTACK));
            }

            for(int i = 0; i < RestCount; i++)
            {
                //Order_Rest order_Rest = new Order_Rest();
                //orderCardList.Add(order_Rest);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OrderCardType.REST));
            }

            for(int i = 0; i < ShuffleCount; i++)
            {
                // Order_Shuffle order_Shuffle = new Order_Shuffle();
                //orderCardList.Add(order_Shuffle);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OrderCardType.SHUFFLE));
            }

            for(int i = 0; i < TakeCount; i++)
            {
                //Order_Take order_Take = new Order_Take();
                //orderCardList.Add(order_Take);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OrderCardType.TAKE));
            }


            //非指令卡10-15张
            int OtherCount = 13;
            int LandLifeCount = 1;//生息之地1张
            int Recovery_siphonCount = 1;//复原虹吸1张       
            int Sky_fireCount = 1;//天火一张                      
            int Ancestor_HelmetsCount = 1;//先祖头盔1
            int Enhanced_ExplosivesCount = 1;//强化炸药1

            int restOther = (OtherCount - LandLifeCount - Recovery_siphonCount - Sky_fireCount - Ancestor_HelmetsCount - Enhanced_ExplosivesCount);//剩余非指令卡
            int Totem_summonCount = restOther / 2;//召唤图腾1张 
            restOther -= Totem_summonCount;
            int Ground_fetter_netCount = Totem_summonCount / 2;//地缚网2
            restOther -= Ground_fetter_netCount;
            int Lightning_ChainCount = Ground_fetter_netCount / 2;//闪电链1
            restOther -= Lightning_ChainCount;
            int Toad_bombCount = restOther;//蟾蜍炸弹

            /*int OtherCount = 56 - 17 - OrderCount;
            int LandLifeCount = 0;//生息之地1张
            int Recovery_siphonCount = 0;//复原虹吸1张
            int Lightning_ChainCount = 0;//闪电链1
            int Sky_fireCount = 0;//天火一张
            int Totem_summonCount = 0;//召唤图腾1张
            int Ground_fetter_netCount = 0;//地缚网2
            int Ancestor_HelmetsCount = OtherCount;//先祖头盔1
            int Enhanced_ExplosivesCount = 0;//强化炸药1
            int Toad_bombCount = 0;*/

            for (int i = 0; i < LandLifeCount; i++)
            {
                //Orc_Other_LandLife orc_Other_LandLife = new Orc_Other_LandLife();
                //otherCardList.Add(orc_Other_LandLife);
                cardDtos.Add(new CardDto(CardId.Add_Get(),OtherCardType.ManorCard,RaceType.ORC,OrcOtherCardType.LandLife));
            }

            for(int i = 0; i < Recovery_siphonCount; i++)
            {
                //Orc_Other_Recoverysiphon orc_Other_Recoverysiphon = new Orc_Other_Recoverysiphon();
                //otherCardList.Add(orc_Other_Recoverysiphon);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.MagicCard, RaceType.ORC, OrcOtherCardType.Recovery_siphon));
            }

            for(int i = 0; i < Lightning_ChainCount; i++)
            {
                //Orc_Other_LightningChain orc_Other_LightningChain = new Orc_Other_LightningChain();
                //otherCardList.Add(orc_Other_LightningChain);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.MagicCard, RaceType.ORC, OrcOtherCardType.Lightning_Chain));
            }

            for(int i = 0; i < Sky_fireCount; i++)
            {
                //Orc_Other_Skyfire orc_Other_Skyfire = new Orc_Other_Skyfire();
                //otherCardList.Add(orc_Other_Skyfire);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.MagicCard, RaceType.ORC, OrcOtherCardType.Sky_fire));
            }

            for(int i = 0; i < Totem_summonCount; i++)
            {
                //Orc_Other_Totemsummon orc_Other_Totemsummon = new Orc_Other_Totemsummon();
                //otherCardList.Add(orc_Other_Totemsummon);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.MagicCard, RaceType.ORC, OrcOtherCardType.Totem_summon));
            }

            for(int i = 0; i < Ground_fetter_netCount; i++)
            {
                //Orc_Other_Groundfetternet orc_Other_Groundfetternet = new Orc_Other_Groundfetternet();
                //otherCardList.Add(orc_Other_Groundfetternet);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.TrapCard, RaceType.ORC, OrcOtherCardType.Ground_fetter_net));
            }

            for(int i = 0; i < Ancestor_HelmetsCount; i++)
            {
                //Orc_Other_AncestorHelmets orc_Other_AncestorHelmets = new Orc_Other_AncestorHelmets();
                //otherCardList.Add(orc_Other_AncestorHelmets);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.EquipCard, RaceType.ORC, OrcOtherCardType.Ancestor_Helmets));
            }

            for(int i = 0; i < Enhanced_ExplosivesCount; i++)
            {
                //Orc_Other_EnhancedExplosives orc_Other_EnhancedExplosives = new Orc_Other_EnhancedExplosives();
                //otherCardList.Add(orc_Other_EnhancedExplosives);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.EquipCard, RaceType.ORC, OrcOtherCardType.Enhanced_Explosives));
            }

            for(int i = 0; i < Toad_bombCount; i++)
            {
                //Orc_Other_Toadbomb orc_Other_Toadbomb = new Orc_Other_Toadbomb();
                //otherCardList.Add(orc_Other_Toadbomb);
                cardDtos.Add(new CardDto(CardId.Add_Get(), OtherCardType.EquipCard, RaceType.ORC, OrcOtherCardType.Toad_bomb));
            }


            /*//把所有类型的牌装入牌库
            foreach (var item in armycardList)
            {
                Cards.Add(item);
            }

            foreach (var item in orderCardList)
            {
                Cards.Add(item);
            }

            foreach (var item in otherCardList)
            {
                Cards.Add(item);
            }*/
            
        }       

        /// <summary>
        /// 洗牌
        /// </summary>
        private void shuffle()
        {
            //List<int> listint = Enumerable.Range(1, 10000000).OrderBy(x => Guid.NewGuid()).Take(100000).ToList();
            //List<ICardBase> shuffleCards1 = new List<ICardBase>();
            //List<ICardBase> shuffleCards2 = new List<ICardBase>();
            List<CardDto> shuffleCards1 = new List<CardDto>();
            List<CardDto> shuffleCards2 = new List<CardDto>();
            //Queue<int> indexqueue = new Queue<int>();
            List<int> indexList = new List<int>();
            bool hasindex = false;


            for (int i = 0; i < 56; i++)
            {
                shuffleCards1.Add(new CardDto());
                shuffleCards2.Add(new CardDto());
                while (true)
                {
                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    int index = random.Next(0, 56);
                    hasindex = indexList.Contains(index);
                    if (hasindex == false)
                    {
                        //indexqueue.Enqueue(index);
                        indexList.Add(index);
                        //Console.WriteLine(index);
                        hasindex = true;
                        break;
                    }
                }
            }


            for(int i = 0; i < 56; i++)
            {
                int index = indexList[i];
                shuffleCards1[i] = playercardDtos[0][index];
                shuffleCards2[i] = playercardDtos[1][55-index];
            }

            /*for (int i = 55; i >= 0; i--)
            {
                int index = indexList[i];
                shuffleCards2[i] = playercardDtos[1][index];
            }*/

            /*for(int i = 0; i < 56; i++)
            {
                int index = indexqueue.Dequeue();
                shuffleCards1[index] = playerCards[0][i];
                shuffleCards2[index] = playerCards[1][i];
            }*/

            //cardDtos.Clear();
            playercardDtos[0].Clear();
            playercardDtos[1].Clear();

            for (int i = 0; i < 56; i++)
            {
                playercardDtos[0].Add(shuffleCards1[i]);
                playercardDtos[1].Add(shuffleCards2[i]);
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        public CardDto DispatchCard(int uid)
        {
            int playerIndex = getIndexByUid(uid);
            if(playerIndex == -1)
            {
                throw new Exception("牌库获取玩家索引出错");
            }

            if (playercardDtos[playerIndex].Count > 0)
            {
                CardDto cardDto = playercardDtos[playerIndex][0];
                playercardDtos[playerIndex].RemoveAt(0);
                return cardDto;
            }
            return null;
        }

        /// <summary>
        /// 给玩家发指定类型的牌
        /// </summary>
        /// <returns></returns>
        public CardDto DispatchCard(int uid , int cardType)
        {
            int playerIndex = getIndexByUid(uid);
            if (playerIndex == -1)
            {
                throw new Exception("牌库获取玩家索引出错");
            }

            if (playercardDtos[playerIndex].Count > 0)
            {
                CardDto cardDto = null;
                //playercardDtos[playerIndex].RemoveAt(0);
                for(int i = 0; i < playercardDtos[playerIndex].Count; i++)
                {
                    if((playercardDtos[playerIndex][i].Type == CardType.ARMYCARD &&
                        playercardDtos[playerIndex][i].Class == ArmyClassType.HighClass)||
                        (playercardDtos[playerIndex][i].Type == CardType.ARMYCARD &&
                        playercardDtos[playerIndex][i].Class == ArmyClassType.MiddleClass))
                    {
                        cardDto = playercardDtos[playerIndex][i];
                        playercardDtos[playerIndex].RemoveAt(i);
                        break;
                    }
                }

                return cardDto;
            }
            return null;
        }

        /// <summary>
        /// 通过玩家ID获得玩家索引
        /// </summary>
        /// <returns></returns>
        private int getIndexByUid(int uid)
        {
            foreach (var item in uid_indexDict)
            {
                if(item.Key == uid)
                {
                    return item.Value;
                }
            }
            return -1;
        }

    }
}
