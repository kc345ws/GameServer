using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 房间数据传输对象
    /// </summary>
    /// 
    [Serializable]
    public class MatchRoomDto
    {
        public int Leftid { get; private set; }
        public int Rightid { get; private set; }
        /// <summary>
        /// 在房间内的角色ID与角色数据模型
        /// </summary>
        public Dictionary<int, UserDto> UidUdtoDic { get; set; }

        /// <summary>
        /// 在房间内准备了的角色ID
        /// </summary>
        public List<int> ReadyUidlist { get; set; }

        /// <summary>
        /// 用户进入房间的顺序
        /// </summary>
        public List<int> EnterList { get; set; }

        public MatchRoomDto() {
            UidUdtoDic = new Dictionary<int, UserDto>();
            ReadyUidlist = new List<int>();
            EnterList = new List<int>();
        }

        /// <summary>
        /// 把新进入房间的用户添加到字典中
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userDto"></param>
        public void Add(int uid , UserDto userDto)
        {
            //向玩家ID与玩家数据模型的映射中添加数据
            UidUdtoDic.Add(uid, userDto);
            //向进入顺序列表中添加数据
            EnterList.Add(uid);
        }

        /// <summary>
        /// 玩家准备
        /// </summary>
        public void Ready(int uid)
        {
            //如果准备了则不能重复准备
            foreach (var item in ReadyUidlist)
            {
                if (item == uid)
                {
                    return;
                }
            }

            ReadyUidlist.Add(uid);
        }

        public void CancelReady(int uid)
        {
            //准备了才能取消准备
            foreach (var item in ReadyUidlist)
            {
                if(item == uid)
                {
                    ReadyUidlist.Remove(uid);
                }
            }     
        }

        /// <summary>
        /// 当有玩家离开
        /// </summary>
        /// <param name="uid"></param>
        public void Leave(int uid)
        {
            UidUdtoDic.Remove(uid);

            EnterList.Remove(uid);
            //如果准备列表中有玩家列表
            foreach (var item in ReadyUidlist)
            {
                if(item == uid)
                {
                    ReadyUidlist.Remove(item);
                }
            }
        }

        /// <summary>
        /// 每当有玩家进入或离开房间重置座位次序
        /// </summary>
        /// <param name="myuid">自己的用户ID</param>
        public void ResetPosition(int myuid)
        {
            Leftid = -1;
            Rightid = -1;
            //一个人的情况不用设置左右玩家
            if(EnterList.Count == 1)
            {
                return;
            }
            else if(EnterList.Count == 2)//两个人
            {
                //如果自己是先进入的 i a
                if(myuid == EnterList[0])
                {
                    Rightid = EnterList[1];
                }
                else//如果自己后进入 a i
                {
                    Leftid = EnterList[0];
                }
            }
            else if(EnterList.Count == 3)//三个人
            {
                //如果自己先进入 i a b
                if(myuid == EnterList[0])
                {
                    Rightid = EnterList[1];
                    Leftid = EnterList[2];
                }else if(myuid == EnterList[1])//如果自己第二个进入a i b
                {
                    Leftid = EnterList[0];
                    Rightid = EnterList[2];
                }
                else//如果自己第三个进入 a b i
                {
                    Leftid = EnterList[1];
                    Rightid = EnterList[0];
                }
            }
        }
    }
}
