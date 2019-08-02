using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Protocol.Code
{
    /// <summary>
    /// 帐号模块子操作码400-500
    /// </summary>
    public static class AccountCode
    {
        /// <summary>
        /// 客户端帐号注册请求
        /// </summary>
        public const int REGISTER_CREQ = 400;

        /// <summary>
        /// 服务器帐号注册回复
        /// </summary>
        public const int REGISTER_SRES = 401;

        /// <summary>
        /// 客户端帐号登陆请求
        /// </summary>
        public const int LOGIN_CREQ = 402;

        /// <summary>
        /// 服务器帐号登陆回复
        /// </summary>
        public const int LOGIN_SRES = 403;
    }
}
