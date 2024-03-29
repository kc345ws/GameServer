﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants.Map
{
    /// <summary>
    /// 地图
    /// </summary>
    public class Map
    {
        /// <summary>
        /// 地图点与兵种卡的映射
        /// </summary>
        public Dictionary<MapPoint, ArmyCardBase> Point_Army_Dict;
        public MapPoint[] mapPoints;

        /// <summary>
        /// 地图点个数
        /// </summary>
        private int MapSize = 9 * 13;

        /// <summary>
        /// 行数
        /// </summary>
        private int RowCount = 9;

        /// <summary>
        /// 列数
        /// </summary>
        private int ColumnCount = 13;

        public Map()
        {
            Point_Army_Dict = new Dictionary<MapPoint, ArmyCardBase>();
            mapPoints = new MapPoint[MapSize];

            Create();
        }

        private void Create()
        {
            int index = 0;
            for (int column = 0; column < ColumnCount; column++)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    mapPoints[index].X = row;
                    mapPoints[index].Z = column;
                }
            }
        }

        public MapPoint GetPosition(ArmyCardBase armyCardBase)
        {
            foreach (var item in Point_Army_Dict)
            {
                if(item.Value == armyCardBase)
                {
                    return item.Key;
                }
            }
            throw new Exception("地图上没有该兵种");
        }
    }
}
