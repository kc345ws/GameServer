using ChcServer;
using ChcServer.Util.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameServer.Cache.Match
{
    /// <summary>
    /// 匹配缓冲区
    /// </summary>
    public class MatchCache
    {
        private static MatchCache instance = new MatchCache();
        public static MatchCache Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                    {
                        instance = new MatchCache();
                    }
                    return instance;
                }        
            }
        }
        
        private MatchCache() { }
        /// <summary>
        /// 正在匹配中的角色ID与房间ID的映射
        /// </summary>
        private Dictionary<int, int> uidRoomidDic = new Dictionary<int, int>();

        /// <summary>
        /// 正在等待中的房间ID与房间模型的映射
        /// </summary>
        private Dictionary<int, MatchRoom> ridModleDic = new Dictionary<int, MatchRoom>();

        /// <summary>
        /// 匹配房间复用
        /// </summary>
        Queue<MatchRoom> matchRooms =new Queue<MatchRoom>();

        private ConcurrentInt roomid = new ConcurrentInt(-1);

        /// <summary>
        /// 角色进入匹配房间后 需要获取到房间内所有人的信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public MatchRoom Enter(int uid , ClientPeer clientPeer)
        {
            //先搜寻是否有等待中的房间
            foreach(MatchRoom room in ridModleDic.Values)
            {
                if (room.IsFull())
                {
                    continue;
                }
                //如果此房间不满
                room.Enter(uid , clientPeer);//角色进入房间
                uidRoomidDic.Add(uid, room.ID);//加入等待用户与房间ID的映射
                return room;
            }

            //如果没有等待中的房间则自己创建房间
            MatchRoom mr = null;
            createRoom(out mr);
            mr.Enter(uid , clientPeer);
            uidRoomidDic.Add(uid, mr.ID);

            return mr;
        }

        private void createRoom(out MatchRoom room)
        {
            //如果有可复用的房间
            if(matchRooms.Count > 0)
            {
                room = matchRooms.Dequeue();
                ridModleDic.Add(room.ID, room);
                return;
            }
            int rid = roomid.Add_Get();

            room = new MatchRoom(rid);
            //matchRooms.Enqueue(room);
            ridModleDic.Add(rid, room);
        }

        public MatchRoom Leave(int uid)
        {
            int roomid = uidRoomidDic[uid];
            MatchRoom room = ridModleDic[roomid];

            room.Leave(uid);
            uidRoomidDic.Remove(uid);

            //如果房间空了
            if (room.IsEmpty())
            {
                matchRooms.Enqueue(room);
                ridModleDic.Remove(roomid);      
            }

            return room;
        }

        /// <summary>
        /// 获取用户正在等待的房间
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public MatchRoom GetRoom(int uid)
        {
            int roomid = uidRoomidDic[uid];
            return ridModleDic[roomid];
        }

        public bool IsMatching(int uid)
        {
            return uidRoomidDic.ContainsKey(uid);
        }

        /// <summary>
        /// 摧毁房间并回收
        /// </summary>
        /// <param name="room"></param>
        public void Destory(MatchRoom room)
        {
            ridModleDic.Remove(room.ID);

            foreach (var uid in room.UidClientpeerDic.Keys)
            {
                uidRoomidDic.Remove(uid);
            }

            room.ReadyUidlist.Clear();
            room.UidClientpeerDic.Clear();
            matchRooms.Enqueue(room);
        }
    }
}
