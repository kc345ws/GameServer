using ChcServer;
using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Match
{
    /// <summary>
    /// 匹配房间
    /// </summary>
    public class MatchRoom
    {
        /// <summary>
        /// 房间的唯一ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 房间最大容量
        /// </summary>
        public int MaxUser { get; private set; }

        /// <summary>
        /// 在房间内的角色ID与连接对象的映射
        /// </summary>
        public Dictionary<int,ClientPeer> UidClientpeerDic { get; private set; }

        /// <summary>
        /// 在房间内准备了的角色ID
        /// </summary>
        public List<int> ReadyUidlist { get; private set; }

        /// <summary>
        /// 默认最大数量为3
        /// </summary>
        /// <param name="id"></param>
        public MatchRoom(int id)
        {
            ID = id;
            UidClientpeerDic = new Dictionary<int, ClientPeer>();
            ReadyUidlist = new List<int>();
            MaxUser = 3;
        }

        public MatchRoom(int id , int maxuser)
        {
            ID = id;
            UidClientpeerDic = new Dictionary<int, ClientPeer>();
            ReadyUidlist = new List<int>();
            MaxUser = maxuser;
        }

        /// <summary>
        /// 角色进入房间
        /// </summary>
        /// <param name="uid"></param>
        public void Enter(int uid , ClientPeer clientPeer)
        {
            UidClientpeerDic.Add(uid, clientPeer);
        }

        /// <summary>
        /// 用户离开房间
        /// </summary>
        /// <param name="uid"></param>
        public void Leave(int uid)
        {
            UidClientpeerDic.Remove(uid);
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="uid"></param>
        public void Ready(int uid)
        {
            ReadyUidlist.Add(uid);
        }

        /// <summary>
        /// 取消准备
        /// </summary>
        /// <param name="uid"></param>
        public void CancelReady(int uid)
        {
            ReadyUidlist.Remove(uid);
        }

        /// <summary>
        /// 房间是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            if(UidClientpeerDic.Count >= MaxUser)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 房间内人数是否为0
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if(UidClientpeerDic.Count <= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 房间内的所有人是否全部准备
        /// </summary>
        /// <returns></returns>
        public bool IsAllReady()
        {
            if(ReadyUidlist.Count >= MaxUser)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 向房间内的所有人广播
        /// </summary>
        /// <param name="room"></param>
        public void Broadcast(int opcode , int subcode , object value,ClientPeer exclientPeer=null)
        {
            SocketMsg mgr = new SocketMsg(opcode, subcode, value);
            byte[] data = EncodeTool.EncodeSocketMgr(mgr);
            byte[] packet = EncodeTool.EncodeMessage(data);//构造数据包

            foreach (var uid in UidClientpeerDic.Keys)
            {     
                ClientPeer clientPeer = UidClientpeerDic[uid];
                if(clientPeer == exclientPeer)
                {
                    continue;//不用给自己广播消息
                }
                clientPeer.StartSend(packet);
            }
        }
    }
}
