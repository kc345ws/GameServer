using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 结束结果数据传输对象
    /// </summary>
    /// 
    [Serializable]
    public class OverDto
    {
        //public int WinIdentity;//获胜身份
        public List<PlayerDto> WinidList;//获胜玩家ID列表
        public int BennCount;

        public OverDto(int winidentity, List<PlayerDto> winlist,int winbeencount) {
            //WinIdentity = winidentity;
            WinidList = winlist;
            BennCount = winbeencount;
        }
    }
}
