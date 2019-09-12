using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 地图移动传输类
    /// </summary>
    [Serializable]
    public class MapMoveDto
    {
        /// <summary>
        /// 兵种原来所在地图点
        /// </summary>
        public MapPoint OriginalMapPoint;
        /// <summary>
        /// 想要移动到的地图点
        /// </summary>
        public MapPoint MoveMapPoint;
        /// <summary>
        /// 所要移动单是否为飞行单位
        /// </summary>
        //public bool CanFly;
        public int MoveType;

        public MapMoveDto() { }

        public MapMoveDto(MapPoint original , MapPoint move , int movetype)
        {
            OriginalMapPoint = original;
            MoveMapPoint = move;
            MoveType = movetype;
        }

        public void Change(MapPoint original, MapPoint move, int movetype)
        {
            OriginalMapPoint = original;
            MoveMapPoint = move;
            MoveType = movetype;
        }
    }
}
