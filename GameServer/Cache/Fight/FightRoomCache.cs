using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChcServer.Util.Concurrent;
using Protocol.Dto.Fight;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 战斗房间缓冲层
    /// </summary>
    public class FightRoomCache
    {
        private static FightRoomCache instance = new FightRoomCache();
        public static FightRoomCache Instance { get
            {
                if(instance == null)
                {
                    instance = new FightRoomCache();
                }
                return instance;
            } }

        public FightRoomCache() { }
        /// <summary>
        /// 房间ID
        /// </summary>
        private ConcurrentInt roomID = new ConcurrentInt(-1);

        /// <summary>
        /// 用户ID与房间ID的映射
        /// </summary>
        private Dictionary<int, int> uidRoomidDic = new Dictionary<int, int>();

        /// <summary>
        /// 房间ID与房间模型的映射
        /// </summary>
        private Dictionary<int, FightRoom> roomidModleDic = new Dictionary<int, FightRoom>();

        /// <summary>
        /// 战斗房间重用
        /// </summary>
        private Queue<FightRoom> fightRoomQueue = new Queue<FightRoom>();

        /// <summary>
        /// 用户是否进入战斗房间
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsJoinFight(int uid)
        {
            return uidRoomidDic.ContainsKey(uid);
        }


        public FightRoom Create(List<int>uidList)
        {
            if(fightRoomQueue.Count > 0)
            {
                //如果重用队列中有可用房间
                FightRoom fightRoom = fightRoomQueue.Dequeue();
                foreach (var item in uidList)
                {
                    uidRoomidDic.Add(item, fightRoom.ID);

                    PlayerDto playerDto = new PlayerDto(item);
                    fightRoom.playerDtos.Add(playerDto);
                }
            
                return fightRoom;
            }
            else
            {
                FightRoom fightRoom = new FightRoom(roomID.Add_Get(), uidList);


                foreach (var item in uidList)
                {
                    if(!uidRoomidDic.ContainsKey(item))
                    {
                        uidRoomidDic.Add(item, roomID.Get());
                    }
                }
                roomidModleDic.Add(roomID.Get(), fightRoom);

                
                return fightRoom;
            }
        }

        /// <summary>
        /// 通过房间ID获取房间数据模型
        /// </summary>
        /// <returns></returns>
        public FightRoom GetRoomById(int id)
        {
            if (roomidModleDic.ContainsKey(id))
            {
                return roomidModleDic[id];
            }
            else
            {
                throw new Exception("没有这个战斗房间");
            }
        }

        public FightRoom GetRoomByUid(int uid)
        {
            int roomid;
            if (uidRoomidDic.ContainsKey(uid))
            {
                roomid = uidRoomidDic[uid];
            }
            else
            {
                throw new Exception("战斗房间列表中没有这个玩家");
            }

            if (roomidModleDic.ContainsKey(roomid))
            {
                return roomidModleDic[roomid];
            }
            else
            {
                throw new Exception("没有这个战斗房间");
            }
        }

        /// <summary>
        /// 销毁房间清空房间内数据并进行重用
        /// </summary>
        /// <param name="roomid"></param>
        public void Destroy(int roomid)
        {
            FightRoom fightRoom = null;
            if (roomidModleDic.ContainsKey(roomid))
            {
                fightRoom = roomidModleDic[roomid];
            }
            else
            {
                throw new Exception("没有这个战斗房间");
            }

            fightRoom.cardLibrary.Init();//重新初始化牌库
            fightRoom.TableCards.Clear();
            fightRoom.roundModle.Init();//初始化回合管理器

            for (int i = 0; i < fightRoom.playerDtos.Count; i++)
            {
                uidRoomidDic.Remove(fightRoom.playerDtos[i].UserID);
            }
            fightRoom.playerDtos.Clear();

            fightRoom.LeavePlayerDtos.Clear();

            

            fightRoomQueue.Enqueue(fightRoom);
        }
    }
}
