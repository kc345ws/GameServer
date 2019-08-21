using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 地图点传送类
    /// </summary>
    [Serializable]
    public class MapPointDto
    {
        /// <summary>
        /// 陆地单位所属种族
        /// </summary>
        public int LandArmyRace;

        /// <summary>
        /// 陆地单位名称
        /// </summary>
        public int LandArmyName;

        /// <summary>
        /// 飞行单位所属种族
        /// </summary>
        public int SkyArmyRace;

        /// <summary>
        /// 飞行单位名称
        /// </summary>
        public int SkyArmyName;

        public MapPoint mapPoint;//地图点

        public MapPointDto() { }

        public MapPointDto(MapPoint point,int landrace ,int landname , int skyrace,int skyname)
        {
            mapPoint = point;
            LandArmyRace = landrace;
            LandArmyName = landname;
            SkyArmyRace = skyrace;
            SkyArmyName = skyname;
        }

        public void Change(MapPoint point, int landrace, int landname, int skyrace, int skyname)
        {
            mapPoint = point;
            LandArmyRace = landrace;
            LandArmyName = landname;
            SkyArmyRace = skyrace;
            SkyArmyName = skyname;
        }
    }
}
