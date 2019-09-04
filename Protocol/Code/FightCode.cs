using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class FightCode
    {
        /*public const int GRAB_LANDLORD_CREQ = 0;
        public const int GRAB_LANDLORD_SBOD = 1;//第一个叫的结果
        public const int TURN_LANDLORD_SBOD = 2;//下一个抢的结果

        public const int DEAL_CREQ = 3;
        public const int DEAL_SRES = 4;
        public const int DEAL_SBOD = 5;

        public const int PASS_CREQ = 6;
        public const int PASS_SRES = 7;

        public const int TURN_DEAL_SBOD = 8;//转换出牌

        public const int PLAYER_LEAVE_SBOD = 9;
        public const int GAME_OVER_SBOD = 10;

        public const int GET_CARD_CREQ = 11;
        public const int GET_CARD_SRES = 12;

        public const int PLAYER_LEAVE_CREQ = 13;//玩家离开房间*/


        /// <summary>
        /// 1兽族 2...3...
        /// </summary>
        public const int SELECT_RACE_CREQ = 0;//选择种族
        public const int SELECT_RACC_SRES = 1;
        public const int SELECT_RACE_SBOD = 2;


        public const int GET_CARD_SRES = 3;//服务器向客户端发送手牌信息

        /// <summary>
        /// 地图上设置兵种
        /// </summary>
        public const int MAP_SET_ARMY_CREQ = 4;
        public const int MAP_SET_ARMY_SRES = 5;
        public const int MAP_SET_ARMY_SBOD = 6;

        /// <summary>
        /// 出牌
        /// </summary>
        public const int DEAL_CARD_CREQ = 7;
        public const int DEAL_CARD_SRES = 8;
        public const int DEAL_CARD_SBOD = 9;

        /// <summary>
        /// 兵种移动
        /// </summary>
        public const int MAP_ARMY_MOVE_CREQ = 10;
        public const int MAP_ARMY_MOVE_SRES = 11;
        public const int MAP_ARMY_MOVE_SBOD = 12;

        /// <summary>
        /// 兵种攻击
        /// </summary>
        public const int ARMY_ATTACK_CREQ = 13;
        public const int ARMY_ATTACK_SRES = 14;
        public const int ARMY_ATTACK_SBOD = 15;

        /// <summary>
        /// 闪避
        /// </summary>
        public const ushort DEAL_DODGE_CREQ = 16;
        public const ushort DEAL_DODGE_SRES = 17;
        public const ushort DEAL_DODGE_SBOD = 18;

    }
}
