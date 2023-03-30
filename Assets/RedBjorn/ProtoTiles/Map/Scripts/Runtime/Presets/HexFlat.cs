using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class HexFlat
    {
        public class Matrix
        {
            public class Fractional
            {
                public const float x11 = 2f / 3f;
                public const float x12 = 0f;
                public const float x21 = -1f / 3f;
                public const float x22 = Constants.Sqrt3 / 3f;
            }

            public class ToWorld
            {
                public const float x11 = 1.5f;
                public const float x12 = 0f;
                public const float x21 = Constants.Cos30;
                public const float x22 = Constants.Sqrt3;
            }
        }

        static readonly Vector3[] VerticesXZ = new Vector3[]
        {
            new Vector3( Constants.Cos60, 0f,  Constants.Cos30),
            new Vector3( 1f,              0f,  0f),
            new Vector3( Constants.Cos60, 0f, -Constants.Cos30),
            new Vector3(-Constants.Cos60, 0f, -Constants.Cos30),
            new Vector3(-1,               0f,  0f),
            new Vector3(-Constants.Cos60, 0f,  Constants.Cos30)
        };

        static readonly Vector3[] VerticesXY = new Vector3[]
        {
            new Vector3( Constants.Cos60,  Constants.Cos30, 0f),
            new Vector3( 1f,               0f,              0f),
            new Vector3( Constants.Cos60, -Constants.Cos30, 0f),
            new Vector3(-Constants.Cos60, -Constants.Cos30, 0f),
            new Vector3(-1,                0f,              0f),
            new Vector3(-Constants.Cos60,  Constants.Cos30, 0f)
        };

        static readonly Vector3[] VerticesZY = new Vector3[]
        {
            new Vector3(0f,  Constants.Cos30,  Constants.Cos60),
            new Vector3(0f,  0f,               1f             ),
            new Vector3(0f, -Constants.Cos30,  Constants.Cos60),
            new Vector3(0f, -Constants.Cos30, -Constants.Cos60),
            new Vector3(0f,  0,               -1f             ),
            new Vector3(0f,  Constants.Cos30, -Constants.Cos60)
        };

        /// <summary>
        /// Vertices of the single hex with side size = 1
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
            return (Array.IndexOf(Hex.Neighbour, direction) + Hex.Neighbour.Length - 1) % Hex.Neighbour.Length;
        }

        public static int VerticeIndexRight(Vector3Int direction)
        {
            return Array.IndexOf(Hex.Neighbour, direction);
        }

        static readonly Vector3[] SideCenterXZ = new Vector3[]
        {
            (VerticesXZ[0] + VerticesXZ[5]) / 2f,
            (VerticesXZ[1] + VerticesXZ[0]) / 2f,
            (VerticesXZ[2] + VerticesXZ[1]) / 2f,
            (VerticesXZ[3] + VerticesXZ[2]) / 2f,
            (VerticesXZ[4] + VerticesXZ[3]) / 2f,
            (VerticesXZ[5] + VerticesXZ[4]) / 2f
        };

        static readonly Vector3[] SideCenterXY = new Vector3[]
        {
            (VerticesXY[0] + VerticesXY[5]) / 2f,
            (VerticesXY[1] + VerticesXY[0]) / 2f,
            (VerticesXY[2] + VerticesXY[1]) / 2f,
            (VerticesXY[3] + VerticesXY[2]) / 2f,
            (VerticesXY[4] + VerticesXY[3]) / 2f,
            (VerticesXY[5] + VerticesXY[4]) / 2f
        };

        static readonly Vector3[] SideCenterZY = new Vector3[]
        {
            (VerticesZY[0] + VerticesZY[5]) / 2f,
            (VerticesZY[1] + VerticesZY[0]) / 2f,
            (VerticesZY[2] + VerticesZY[1]) / 2f,
            (VerticesZY[3] + VerticesZY[2]) / 2f,
            (VerticesZY[4] + VerticesZY[3]) / 2f,
            (VerticesZY[5] + VerticesZY[4]) / 2f
        };

        /// <summary>
        /// Points of hex side centers with side size = 1
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

        /// <summary>
        /// Convert position from hex cube coordinates to world space coordinates
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector3 ToWorld(Vector3Int hex, GridAxis axis, float size = 1f)
        {
            var q = (Matrix.ToWorld.x11 * hex.x + Matrix.ToWorld.x12 * hex.y) * size;
            var r = (Matrix.ToWorld.x21 * hex.x + Matrix.ToWorld.x22 * hex.y) * size;
            if (axis == GridAxis.XZ)
            {
                return new Vector3(q, 0f, r);
            }
            else if (axis == GridAxis.XY)
            {
                return new Vector3(q, r, 0f);
            }
            else if (axis == GridAxis.ZY)
            {
                return new Vector3(0f, r, q);
            }
            return Vector3.zero;
        }

        static Vector3 ToHexagonalFractional(Vector3 point, GridAxis axis, float size)
        {
            float q = 0f;
            float r = 0f;
            if (axis == GridAxis.XZ)
            {
                q = (Matrix.Fractional.x11 * point.x + Matrix.Fractional.x12 * point.z) / size;
                r = (Matrix.Fractional.x21 * point.x + Matrix.Fractional.x22 * point.z) / size;
            }
            else if (axis == GridAxis.XY)
            {
                q = (Matrix.Fractional.x11 * point.x + Matrix.Fractional.x12 * point.y) / size;
                r = (Matrix.Fractional.x21 * point.x + Matrix.Fractional.x22 * point.y) / size;
            }
            else if (axis == GridAxis.ZY)
            {
                q = (Matrix.Fractional.x11 * point.z + Matrix.Fractional.x12 * point.y) / size;
                r = (Matrix.Fractional.x21 * point.z + Matrix.Fractional.x22 * point.y) / size;
            }
            return new Vector3(q, r, -q - r);
        }

        /// <summary>
        /// Convert position from world space coordinates to hex cube integer coordinates
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector3Int ToHexagonal(Vector3 point, GridAxis axis, float size = 1f)
        {
            return Hex.HexNearest(ToHexagonalFractional(point, axis, size));
        }

        /// <summary>
        /// Center position of nearest hex in world space coordinates
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Vector3 Center(Vector3 position, GridAxis axis, float size = 1f)
        {
            return ToWorld(ToHexagonal(position, axis, size), axis, size);
        }

        /// <summary>
        /// Index of neighbour direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>Index starting from 6th vertice</returns>
        public static int NeighbourTileIndexAtDirection(Vector3 direction, GridAxis axis)
        {
            var v1 = Vertices(axis)[5];
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
            return Mathf.FloorToInt(angle / 60f);
        }
    }
}
