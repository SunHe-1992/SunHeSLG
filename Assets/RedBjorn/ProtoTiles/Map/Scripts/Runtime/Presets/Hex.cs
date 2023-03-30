using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class Hex
    {
        /// <summary>
        /// Epsilon
        /// </summary>
        public static readonly Vector3 Eps = new Vector3(1e-6f, 2e-6f, -3e-6f);

        /// <summary>
        /// Neighbour directions with side size = 1 (in hex cube integer coordinates)
        /// </summary>
        public static readonly Vector3Int[] Neighbour = new Vector3Int[]
        {
            new Vector3Int( 0, 1,-1),
            new Vector3Int( 1, 0,-1),
            new Vector3Int( 1,-1, 0),
            new Vector3Int( 0,-1, 1),
            new Vector3Int(-1, 0, 1),
            new Vector3Int(-1, 1, 0)
        };

        public static readonly float[] SideRotation = new float[]
        {
             90f,
            -30f,
             30f,
             90f,
            -30f,
             30f
        };

        /// <summary>
        /// Distance between two hexes with side size = 1 (in hex cube integer coordinates)
        /// </summary>
        public static readonly float DistanceBetweenCenters = Constants.Sqrt3;

        /// <summary>
        /// Distance between two hexes with corresponding positions with side size = 1 (in hex cube integer coordinates)
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <returns></returns>
        public static float Distance(Vector3Int coord1, Vector3Int coord2)
        {
            return Mathf.Max(Math.Abs(coord1.x - coord2.x), Math.Abs(coord1.y - coord2.y), Math.Abs(coord1.z - coord2.z));
        }

        /// <summary>
        /// Hexes in the area around origin and no further than the range (in hex cube integer coordinates)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns>hex cube integer coordinates</returns>
        public static List<Vector3Int> Area(Vector3Int origin, float range)
        {
            if (range <= 0f)
            {
                return new List<Vector3Int> { origin };
            }
            var area = new List<Vector3Int>();
            var rangeBorder = Mathf.FloorToInt(range);
            for (int x = -rangeBorder; x <= range; x++)
            {
                var yMin = Mathf.Max(-rangeBorder, -x - rangeBorder);
                var yMax = Mathf.Min( rangeBorder, -x + rangeBorder);
                for (int y = yMin; y <= yMax; y++)
                {
                    area.Add(new Vector3Int(origin.x + x, origin.y + y, origin.z - x - y));
                }
            }
            return area;
        }

        /// <summary>
        /// Nearest hex to the point in world space coordinates
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3Int HexNearest(Vector3 point)
        {
            var roundX = Mathf.RoundToInt(point.x);
            var roundY = Mathf.RoundToInt(point.y);
            var roundZ = Mathf.RoundToInt(point.z);

            var diffX = Mathf.Abs(roundX - point.x);
            var diffY = Mathf.Abs(roundY - point.y);
            var diffZ = Mathf.Abs(roundZ - point.z);

            if (diffX > diffY && diffX > diffZ)
            {
                roundX = -roundZ - roundY;
            }
            else if (diffY > diffZ)
            {
                roundY = -roundX - roundZ;
            }
            else
            {
                roundZ = -roundX - roundY;
            }
            return new Vector3Int(roundX, roundY, roundZ);
        }
    }
}

