using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 回合管理类
    /// </summary>
    public class RoundModle
    {
        /// <summary>
        /// 当前出牌者ID
        /// </summary>
        public int CurrentUid { get; set; }

        /// <summary>
        /// 当前最大出牌者ID
        /// </summary>
        public int BiggestUid { get; set; }

        /// <summary>
        /// 上一个最大出牌的长度
        /// </summary>
        public int LastLength { get; set; }

        /// <summary>
        /// 上一个最大出牌的权值
        /// </summary>
        public int LastWeight { get; set; }

        /// <summary>
        /// 上一个最大出牌的类型
        /// </summary>
        public int LastType { get; set; }


        public RoundModle()
        {
            BiggestUid = -1;
            CurrentUid = -1;
            LastType = -1;
            LastLength = -1;
            LastWeight = -1;
        }

        public void Init()
        {
            BiggestUid = -1;
            CurrentUid = -1;
            LastType = -1;
            LastLength = -1;
            LastWeight = -1;
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="uid">开始玩家的ID</param>
        public void Start(int uid)
        {
            BiggestUid = uid;
            CurrentUid = uid;
        }

        /// <summary>
        /// 改变当前最大出牌者,更新回合数据
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="length"></param>
        /// <param name="weight"></param>
        /// <param name="color"></param>
        public void Change(int uid, int length,int weight,int type)
        {
            BiggestUid = uid;
            LastLength = length;
            LastWeight = weight;
            LastType = type;
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        /// <param name="uid"></param>
        public void Turn(int uid)
        {
            CurrentUid = uid;
        }
    }
}
