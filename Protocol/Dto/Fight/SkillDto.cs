using Protocol.Constants.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class SkillDto
    {
        public int Race;//种族
        public int Name;

        public MapPoint TargetMapPoint;

        public int TargetName;//目标单位名称ID


        public SkillDto()
        {

        }

        public SkillDto(int race , int name , int targetName , MapPoint targetMapPoint)
        {
            Race = race;
            Name = name;
            TargetName = targetName;
            TargetMapPoint = targetMapPoint;
        }

        public void Change(int race, int name , int targetName, MapPoint targetMapPoint)
        {
            Race = race;
            Name = name;
            TargetName = targetName;
            TargetMapPoint = targetMapPoint;
        }
    }
}
