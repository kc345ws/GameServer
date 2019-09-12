using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Map
{
    /// <summary>
    /// 地图移动类型
    /// </summary>
    public class MapMoveType
    {
        private static MapMoveType instance;
        public static MapMoveType Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapMoveType();
                }
                return instance;
            }
        }

        private MapMoveType() { }


        /// <summary>
        /// 无法移动
        /// </summary>
        public const int NONE = 0;

        /// <summary>
        /// 左右前移动
        /// </summary>
        public const int Triple_lattice = 1;

        /// <summary>
        /// 前后左右移动
        /// </summary>
        public const int Four_lattice = 2;

        /// <summary>
        /// 前后左右移动两格
        /// </summary>
        public const int Four_lattice_Double = 3;

        /// <summary>
        /// 前后左右移动三格
        /// </summary>
        public const int Four_lattice_Three = 4;

        /// <summary>
        /// 获得移动范围
        /// </summary>
        /// <param name="armyCardBase"></param>
        /// <returns></returns>
        public List<MapPoint> GetMoveRange(ArmyCardBase armyCardBase)
        {
            //可以移动到的范围
            List<MapPoint> canMovePoint = new List<MapPoint>();
            switch (armyCardBase.MoveRangeType)
            {
                case NONE:
                    return null;

                case Triple_lattice:
                    Triple_latticeType(armyCardBase.Position, ref canMovePoint);
                    break;

                case Four_lattice:
                    Four_latticeType(armyCardBase.Position, ref canMovePoint);
                    break;

                case Four_lattice_Double:
                    Four_latticeType_Double(armyCardBase.Position, ref canMovePoint);
                    break;

                case Four_lattice_Three:
                    Four_latticeType_Three(armyCardBase.Position, ref canMovePoint);
                    break;
            }
            return canMovePoint;
        }

        /// <summary>
        /// 判断移动是否超出地图边界
        /// </summary>
        /// <param name="mapPoint"></param>
        /// <returns></returns>
        private bool canMove(MapPoint mapPoint)
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


            if (canMove(new MapPoint(x + 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z));
            }
            if (canMove(new MapPoint(x, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 1));
            }
            if (canMove(new MapPoint(x, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 1));
            }
        }

        private void Four_latticeType(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canMove(new MapPoint(x + 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z));
            }
            if (canMove(new MapPoint(x - 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x - 1, z));
            }
            if (canMove(new MapPoint(x, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 1));
            }
            if (canMove(new MapPoint(x, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 1));
            }
        }

        private void Four_latticeType_Double(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canMove(new MapPoint(x + 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z));
            }
            if (canMove(new MapPoint(x + 2, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 2, z));
            }
            if (canMove(new MapPoint(x - 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x - 1, z));
            }
            if (canMove(new MapPoint(x - 2, z)))
            {
                canAttckPoint.Add(new MapPoint(x  - 2, z));
            }

            if (canMove(new MapPoint(x, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z +1));
            }
            if (canMove(new MapPoint(x, z + 2)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 2));
            }
            if (canMove(new MapPoint(x, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 1));
            }
            if (canMove(new MapPoint(x, z - 2)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 2));
            }
        }

        private void Four_latticeType_Three(MapPoint mapPoint, ref List<MapPoint> canAttckPoint)
        {
            int x = mapPoint.X;
            int z = mapPoint.Z;

            if (canMove(new MapPoint(x + 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 1, z));
            }
            if (canMove(new MapPoint(x + 2, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 2, z));
            }
            if (canMove(new MapPoint(x + 3, z)))
            {
                canAttckPoint.Add(new MapPoint(x + 3, z));
            }
            if (canMove(new MapPoint(x - 1, z)))
            {
                canAttckPoint.Add(new MapPoint(x - 1, z));
            }
            if (canMove(new MapPoint(x - 2, z)))
            {
                canAttckPoint.Add(new MapPoint(x - 2, z));
            }
            if (canMove(new MapPoint(x - 3, z)))
            {
                canAttckPoint.Add(new MapPoint(x - 3, z));
            }


            if (canMove(new MapPoint(x, z + 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 1));
            }
            if (canMove(new MapPoint(x, z + 2)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 2));
            }
            if (canMove(new MapPoint(x, z + 3)))
            {
                canAttckPoint.Add(new MapPoint(x, z + 3));
            }
            if (canMove(new MapPoint(x, z - 1)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 1));
            }
            if (canMove(new MapPoint(x, z - 2)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 2));
            }
            if (canMove(new MapPoint(x, z - 3)))
            {
                canAttckPoint.Add(new MapPoint(x, z - 3));
            }
        }

    }
}
