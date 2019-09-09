using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class FightCode
    {
        /// <summary>
        /// 1兽族 2...3...
        /// </summary>
        public const int SELECT_RACE_CREQ = 0;//选择种族
        public const int SELECT_RACC_SRES = 1;
        public const int SELECT_RACE_SBOD = 2;

        /// <summary>
        /// 服务器向客户端发送手牌信息
        /// </summary>
        public const int GET_CARD_SRES = 3;

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

        /// <summary>
        /// 反击
        /// </summary>
        public const ushort DEAL_BACKATTACK_CREQ = 19;
        public const ushort DEAL_BACKATTACK_SRES = 20;
        public const ushort DEAL_BACKATTACK_SBOD = 21;

        /// <summary>
        /// 修养卡
        /// </summary>
        public const ushort DEAL_REST_CREQ = 22;
        public const ushort DEAL_REST_SRES = 23;
        public const ushort DEAL_REST_SBOD = 24;

        /// <summary>
        /// 使用非指令卡
        /// </summary>
        public const ushort USE_OTHERCARD_CREQ = 25;
        public const ushort USE_OTHERCARD_SRES = 26;
        public const ushort USE_OTHERCARD_SBOD = 27;

        /// <summary>
        /// 使用攻击卡
        /// </summary>
        public const ushort DEAL_ATTACK_CREQ = 28;
        public const ushort DEAL_ATTACK_SRES = 29;
        public const ushort DEAL_ATTACK_SBOD = 30;

        /// <summary>
        /// 下一回合
        /// </summary>
        public const ushort NEXT_TURN_CREQ = 31;
        public const ushort NEXT_TURN_SRES = 32;
        public const ushort NEXT_TURN_SBOD = 33;
    }
}
