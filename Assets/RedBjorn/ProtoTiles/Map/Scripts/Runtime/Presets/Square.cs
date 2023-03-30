using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class Square
    {
        /// <summary>
        /// Epsilon
        /// </summary>
        public static readonly Vector3 Eps = new Vector3(1e-6f, 1e-6f, 1e-6f);

        /// <summary>
        /// Neighbour directions with side size = 1
        /// </summary>
        public static readonly Vector3Int[] Neighbour = new Vector3Int[]
        {
            new Vector3Int( 0, 0, 1),
            new Vector3Int( 1, 0, 0),
            new Vector3Int( 0, 0,-1),
            new Vector3Int(-1, 0, 0),
        };

        static readonly Vector3[] VerticesXZ = new Vector3[]
        {
            new Vector3( 0.5f, 0f, 0.5f),
            new Vector3( 0.5f, 0f,-0.5f),
            new Vector3(-0.5f, 0f,-0.5f),
            new Vector3(-0.5f, 0f, 0.5f)
        };

        static readonly Vector3[] VerticesXY = new Vector3[]
        {
            new Vector3( 0.5f, 0.5f, 0f),
            new Vector3( 0.5f,-0.5f, 0f),
            new Vector3(-0.5f,-0.5f, 0f),
            new Vector3(-0.5f, 0.5f, 0f)
        };

        static readonly Vector3[] VerticesZY = new Vector3[]
        {
            new Vector3( 0f, 0.5f, 0.5f),
            new Vector3( 0f,-0.5f, 0.5f),
            new Vector3( 0f,-0.5f,-0.5f),
            new Vector3( 0f, 0.5f,-0.5f)
        };

        /// <summary>
        /// Vertices of the single square with side size = 1
        /// </summary>
        public static Vector3[] Vertices(GridAxis axis)
        {
            switch (axis)
            {
                case GridAxis.XZ: return VerticesXZ;
                case GridAxis.XY: return VerticesXY;
                case GridAxis.ZY: return VerticesZY;
            }
            return null;
        }

        public static int VerticeIndexLeft(Vector3Int direction)
        {
            return (Array.IndexOf(Neighbour, direction) + Neighbour.Length - 1) % Neighbour.Length;
        }

        public static int VerticeIndexRight(Vector3Int direction)
        {
            return Array.IndexOf(Neighbour, direction);
        }

        static readonly Vector3[] SideCenterXZ = new Vector3[]
        {
            (VerticesXZ[0] + VerticesXZ[3]) / 2f,
            (VerticesXZ[1] + VerticesXZ[0]) / 2f,
            (VerticesXZ[2] + VerticesXZ[1]) / 2f,
            (VerticesXZ[3] + VerticesXZ[2]) / 2f,
        };

        static readonly Vector3[] SideCenterXY = new Vector3[]
        {
            (VerticesXY[0] + VerticesXY[3]) / 2f,
            (VerticesXY[1] + VerticesXY[0]) / 2f,
            (VerticesXY[2] + VerticesXY[1]) / 2f,
            (VerticesXY[3] + VerticesXY[2]) / 2f,
        };

        static readonly Vector3[] SideCenterZY = new Vector3[]
        {
            (VerticesZY[0] + VerticesZY[3]) / 2f,
            (VerticesZY[1] + VerticesZY[0]) / 2f,
            (VerticesZY[2] + VerticesZY[1]) / 2f,
            (VerticesZY[3] + VerticesZY[2]) / 2f,
        };

        /// <summary>
        /// Points of square side centers with side size = 1
        /// </summary>
        public static Vector3[] SideCenter(GridAxis axis)
        {
            switch (axis)
            {
                case GridAxis.XZ: return SideCenterXZ;
                case GridAxis.XY: return SideCenterXY;
                case GridAxis.ZY: return SideCenterZY;
            }
            return null;
        }

        public static readonly float[] SideRotation = new float[] {
             90f,
              0f,
             90f,
              0f,
        };

        /// <summary>
        /// Distance between two squares with side size = 1
        /// </summary>
        public static readonly float DistanceBetweenCenters = 1f;

        /// <summary>
        /// Distance between two squares with corresponding positions with side size = 1
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <returns></returns>
        public static float Distance(Vector3Int coord1, Vector3Int coord2)
        {
            return Vector3Int.Distance(coord1, coord2);
        }

        /// <summary>
        /// Positions of squares in area around origin at max range with side size = 1
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns>integer coordinates</returns>
        public static List<Vector3Int> Area(Vector3Int origin, float range)
        {
            if (range <= 0f)
            {
                return new List<Vector3Int> { origin };
            }
            var rangeSqr = range * range;
            var rangeBorder = Mathf.CeilToInt(range);
            var area = new List<Vector3Int>();
            for (int x = -rangeBorder; x <= rangeBorder; x++)
            {
                for (int z = -rangeBorder; z <= rangeBorder; z++)
                {
                    var v = new Vector3Int(x, 0, z);
                    if (v.sqrMagnitude <= rangeSqr)
                    {
                        area.Add(origin + v);
                    }
                }
            }
            return area;
        }

        /// <summary>
        /// Convert position in integer coordinates to world space coordinates
        /// </summary>
        /// <param name="square"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector3 ToWorld(Vector3Int square, GridAxis axis, float size = 1f)
        {
            switch (axis)
            {
                case GridAxis.XZ: return new Vector3(square.x * size, 0f, square.z * size);
                case GridAxis.XY: return new Vector3(square.x * size, square.z * size, 0f);
                case GridAxis.ZY: return new Vector3(0f, square.z * size, square.x * size);
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Convert position in world space coordinates to integer coordinates
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector3Int ToSquare(Vector3 point, GridAxis axis, float size = 1f)
        {
            switch (axis)
            {
                case GridAxis.XZ: return new Vector3Int(Mathf.RoundToInt(point.x / size), 0, Mathf.RoundToInt(point.z / size));
                case GridAxis.XY: return new Vector3Int(Mathf.RoundToInt(point.x / size), 0, Mathf.RoundToInt(point.y / size));
                case GridAxis.ZY: return new Vector3Int(Mathf.RoundToInt(point.z / size), 0, Mathf.RoundToInt(point.y / size));
            }
            return Vector3Int.zero;
        }

        /// <summary>
        /// Center position of nearest square in world space coordinates
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector3 Center(Vector3 position, GridAxis axis, float size = 1f)
        {
            return ToWorld(ToSquare(position, axis, size), axis);
        }

        /// <summary>
        /// Index of neighbour direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>Index starting from 4th vertice</returns>
        public static int NeighbourTileIndexAtDirection(Vector3 direction, GridAxis axis)
        {
            var v1 = Vertices(axis)[3];
            Vector3 v2 = Vector3.zero;
            switch (axis)
            {
                case GridAxis.XZ: v2 = new Vector3(direction.x, 0f, direction.z); break;
                case GridAxis.XY: v2 = new Vector3(direction.x, direction.y, 0f); break;
                case GridAxis.ZY: v2 = new Vector3(0f, direction.y, direction.z); break;
            }
            var angle = Vector3.Angle(v1, v2);
            switch (axis)
            {
                case GridAxis.XZ: angle = Mathf.Sign(Vector3.Cross(v1, v2).y) < 0 ? 360 - angle : angle; break;
                case GridAxis.XY: angle = Mathf.Sign(Vector3.Cross(v2, v1).z) < 0 ? 360 - angle : angle; break;
                case GridAxis.ZY: angle = Mathf.Sign(Vector3.Cross(v1, v2).x) < 0 ? 360 - angle : angle; break;
            }
            return Mathf.FloorToInt(angle / 90f);
        }
    }
}