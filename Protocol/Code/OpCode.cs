using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Protocol.Code
{
    /// <summary>
    /// 操作码 服务器与客户端共用0-100
    /// </summary>
    public static class OpCode
    {
        /// <summary>
        /// 帐号模块
        /// </summary>
        public const int ACCOUNT = 0;

        /// <summary>
        /// 角色模块
        /// </summary>
        public const int USER = 1;

        /// <summary>
        /// 匹配模块
        /// </summary>
        public const int MATCH = 2;
    }
}
