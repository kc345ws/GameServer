using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 非指令卡基类
    /// </summary>
    public abstract class IOtherCardBase : CardBase
    {
        public int OtherType {get;set;}
        public void Effect() { }
    }
}
