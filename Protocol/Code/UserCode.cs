using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 有关用户的一些操作码
    /// </summary>
    /// 600-699
    public class UserCode
    {
        //获取信息
        public const int GET_USER_CREQ = 600;
        public const int GET_USER_SRES = 601;

        //创建角色
        public const int CREATE_USER_CREQ = 602;
        public const int CREATE_USER_SRES = 603;

        //角色上线
        public const int LOGIN_USER_CREQ = 604;
        public const int LOGIN_USER_SRES = 605;
    }
}
