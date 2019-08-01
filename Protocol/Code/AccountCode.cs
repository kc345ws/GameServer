using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Protocol.Code
{
    /// <summary>
    /// 帐号模块子操作码
    /// </summary>
    public static class AccountCode
    {
        /// <summary>
        /// 客户端帐号注册请求
        /// </summary>
        public const int REGISTER_CREQ = 0;

        /// <summary>
        /// 服务器帐号注册回复
        /// </summary>
        public const int REGISTER_SRES = 1;

        /// <summary>
        /// 客户端帐号登陆请求
        /// </summary>
        public const int LOGIN_CREQ = 2;

        /// <summary>
        /// 服务器帐号登陆回复
        /// </summary>
        public const int LOGIN_SRES = 3;
    }
}
