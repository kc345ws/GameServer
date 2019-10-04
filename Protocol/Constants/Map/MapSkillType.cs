using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Map
{
    public class MapSkillType
    {
        private static MapSkillType instance;
        public static MapSkillType Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapSkillType();
                }
                return instance;
            }
        }

        private MapSkillType() { }
        /// <summary>
        /// 技能距离为0
        /// </summary>
        public const int NONE = 0;

        /// <summary>
        /// 左右前攻击
        /// </summary>
        public const int Triple_lattice = 1;

        /// <summary>
        /// 前后左右攻击
        /// </summary>
        public const int Four_lattice = 2;

        /// <summary>
        /// 向前攻击三格
        /// </summary>
        public const int Three_Front = 3;

        /// <summary>
        /// 自身周围四个角攻击
        /// </summary>
        public const int Four_Angle = 4;

        /// <summary>
        /// 自身周围8格攻击
        /// </summary>
        public const int All_Around = 5;

        /// <summary>
        /// 斜三格
        /// </summary>
        public const int Three_Oblique = 6;

        public List<MapPoint> GetSkillRange(ArmyCardBase armyCardBase)
        {
            //技能可以使用到的范围
            List<MapPoint> canSkillPoint = new List<MapPoint>();
            switch (armyCardBase.SkillRangeType)
            {
                case NONE:
                    return null;

                case Triple_lattice:
                    Triple_latticeType(armyCardBase.Position, ref canSkillPoint);
                    break;

                case Four_lattice:
                    Four_latticeType(armyCardBase.Position, ref canSkillPoint);
                    break;

                case Three_Front:
                    Three_FrontType(armyCardBase.Position, ref canSkillPoint);
                    break;

                case Four_Angle:
                    Four_AngleType(armyCardBase.Position, ref canSkillPoint);
                    break;

                case All_Around:
                    All_AroundType(armyCardBase.Position, ref canSkillPoint);
                    break;

                case Three_Oblique:
                    Three_ObliqueType(armyCardBase.Position, ref canSkillPoint);
                    break;
                
            }
            return canSkillPoint;
            //throw new Exception("没有该兵种的攻击范围类型");
        }

        /// <summary>
        /// 判断攻击点是否超出地图边界
        /// </summary>
        /// <param name="mapPoint"></param>
        /// <returns></returns>
        private bool canAttack(MapPoint mapPoint)
        {
            if (mapPoint.X < 0 || mapPoint.X > 12)
            {
                return false;
            }
            else if (mapPoint.Z < 0 || mapPoint.Z > 8)
            {
                return false;
            }
            return true;
        }

        private void Triple_latticeType(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;


            if (canAttack(new MapPoint(x + 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z));
            }
            if (canAttack(new MapPoint(x, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 1));
            }
            if (canAttack(new MapPoint(x, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 1));
            }
        }

        private void Four_latticeType(MapPoint mapPoint, ref List<MapPoint> canSkillPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canAttack(new MapPoint(x + 1, z)))
            {
                canSkillPoint.Add(new MapPoint(x + 1, z));
            }
            if (canAttack(new MapPoint(x - 1, z)))
            {
                canSkillPoint.Add(new MapPoint(x - 1, z));
            }
            if (canAttack(new MapPoint(x, z + 1)))
            {
                canSkillPoint.Add(new MapPoint(x, z + 1));
            }
            if (canAttack(new MapPoint(x, z - 1)))
            {
                canSkillPoint.Add(new MapPoint(x, z - 1));
            }
        }

        private void Three_FrontType(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canAttack(new MapPoint(x + 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z));
            }
            if (canAttack(new MapPoint(x + 2, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 2, z));
            }
            if (canAttack(new MapPoint(x + 3, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 3, z));
            }

        }

        private void Four_AngleType(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canAttack(new MapPoint(x - 1, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x - 1, z - 1));
            }
            if (canAttack(new MapPoint(x + 1, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z + 1));
            }
            if (canAttack(new MapPoint(x + 1, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z - 1));
            }
            if (canAttack(new MapPoint(x - 1, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x - 1, z + 1));
            }
        }

        private void All_AroundType(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            Four_latticeType(mapPoint, ref canAttckPoint);
            Four_AngleType(mapPoint, ref canAttckPoint);
        }

        private void Three_ObliqueType(MapPoint mapPoint,ref List<MapPoint> canskillPoints)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canAttack(new MapPoint(x + 1, z + 1)))
            {
                canskillPoints.Add(new MapPoint(x + 1, z + 1));
            }
            if (canAttack(new MapPoint(x + 2, z + 2)))
            {
                canskillPoints.Add(new MapPoint(x + 2, z + 2));
            }
            if (canAttack(new MapPoint(x + 3, z + 3)))
            {
                canskillPoints.Add(new MapPoint(x + 3, z + 3));
            }
            if (canAttack(new MapPoint(x - 1, z - 1)))
            {
                canskillPoints.Add(new MapPoint(x - 1, z - 1));
            }
            if (canAttack(new MapPoint(x - 2, z - 2)))
            {
                canskillPoints.Add(new MapPoint(x - 2, z - 2));
            }
            if (canAttack(new MapPoint(x - 3, z - 3)))
            {
                canskillPoints.Add(new MapPoint(x - 3, z - 3));
            }
            if (canAttack(new MapPoint(x + 1, z - 1)))
            {
                canskillPoints.Add(new MapPoint(x + 1, z - 1));
            }
            if (canAttack(new MapPoint(x + 2, z - 2)))
            {
                canskillPoints.Add(new MapPoint(x + 2, z - 2));
            }
            if (canAttack(new MapPoint(x + 3, z - 3)))
            {
                canskillPoints.Add(new MapPoint(x + 3, z - 3));
            }
            if (canAttack(new MapPoint(x - 1, z + 1)))
            {
                canskillPoints.Add(new MapPoint(x - 1, z + 1));
            }
            if (canAttack(new MapPoint(x - 2, z + 2)))
            {
                canskillPoints.Add(new MapPoint(x - 2, z + 2));
            }
            if (canAttack(new MapPoint(x - 3, z + 3)))
            {
                canskillPoints.Add(new MapPoint(x - 3, z + 3));
            }
        }
    }

}
