using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 非指令卡基类
    /// </summary>
    public interface IOtherCardBase : ICardBase
    {
        int OtherType {get;set;}
        void Effect();
    }
}
