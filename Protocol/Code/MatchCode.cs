using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 有关匹配的一些操作码
    /// </summary>
    /// 700-799
    public class MatchCode
    {
        //进入匹配队列
        public const int ENTER_CREQ = 700;//某个客户端请求进入匹配队列
        public const int ENTER_SRES = 701;//服务器回复客户端请求
        public const int ENTER_BOD = 707;//向房间内的所有人广播有玩家进入

        //离开匹配队列
        public const int LEAVE_CREQ = 702;//某个客户端请求离开匹配队列
        public const int LEAVE_BOD = 703;//服务器通知所有匹配队列中的客户端有客户端离开匹配队列

        //准备
        public const int READY_CREQ = 704;//客户端请求准备
        public const int READY_BOD = 705;//服务器通知所有匹配队列中的客户端有客户端准备了

        //开始游戏
        public const int START_GAME_BOD = 706;//当匹配队列中的所有客户端都准备时，服务器向所有客户端发送开始游戏操作码


    }
}
