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


        public const int GET_CARD_SRES = 3;//服务器向客户端发送
    }
}
