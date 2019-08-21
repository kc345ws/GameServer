using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Map
{
    /// <summary>
    /// 地图点
    /// </summary>
    /// 
    [Serializable]
    public class MapPoint
    {
        public int X;
        public int Z;

        public MapPoint() { }

        public MapPoint(int x , int z)
        {
            X = x;
            Z = z;
        }
    }
}
