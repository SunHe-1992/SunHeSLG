using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapBorder
    {
        public struct BorderPoint
        {
            public Vector3Int TilePos;
            public int VerticeIndex;
        }

        public struct Step
        {
            public Vector3Int Direction;
            public Vector3Int Position;
        }

        /// <summary>
        /// Provide position of tile, which is located on a walkable border and corresponding vertices index 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="inside"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static HashSet<BorderPoint> FindBorderPositions(IMapDirectional map, HashSet<Vector3Int> inside, List<Step> steps = null)
        {
            var neighbourDirections = map.NeighboursDirection;
            var positionStart = inside.Aggregate((v1, v2) => v1.z > v2.z ? v1 : v2);
            var directionCurrent = Vector3Int.zero;
            var directionFinish = Vector3Int.zero;
            var outsideNeighbour = false;
            for (int i = 0; i < neighbourDirections.Length; i++)
            {
                var neighPos = positionStart + neighbourDirections[i];
                if (!inside.Contains(neighPos))
                {
                    outsideNeighbour = true;
                    directionFinish = neighbourDirections[i];
                    directionCurrent = directionFinish;
                    break;
                }
            }

            var border = new HashSet<BorderPoint>();
            if (outsideNeighbour)
            {
                border.Add(new BorderPoint { TilePos = positionStart, VerticeIndex = map.VerticeLeft(directionCurrent) });
                border.Add(new BorderPoint { TilePos = positionStart, VerticeIndex = map.VerticeRight(directionCurrent) });
                var positionPrevious = positionStart;
                var positionCurrent = positionStart + directionCurrent;
                if (steps != null)
                {
                    steps.Add(new Step { Direction = directionCurrent, Position = positionPrevious });
                }

                var i = 0;
                do
                {
                    if (i > 10000)
                    {
                        Log.E("FindBorderPositions algorithm failed or there is too big walkable area to provide border positions within reasonable time");
                        break;
                    }
                    i++;
                    var verticeLeft = map.VerticeLeft(directionCurrent);
                    var verticeRight = map.VerticeRight(directionCurrent);
                    if (inside.Contains(positionCurrent))
                    {
                        directionCurrent = map.TurnLeft(directionCurrent);
                    }
                    else
                    {
                        directionCurrent = map.TurnRight(directionCurrent);
                    }
                    var goInside = inside.Contains(positionPrevious) && !inside.Contains(positionCurrent);
                    var goOutside = !inside.Contains(positionPrevious) && inside.Contains(positionCurrent);
                    if (goOutside)
                    {
                        border.Add(new BorderPoint { TilePos = positionPrevious, VerticeIndex = verticeLeft });
                    }
                    if (goInside)
                    {
                        border.Add(new BorderPoint { TilePos = positionPrevious, VerticeIndex = verticeRight });
                    }

                    positionPrevious = positionCurrent;
                    positionCurrent = positionCurrent + directionCurrent;
                    if (steps != null)
                    {
                        steps.Add(new Step { Direction = directionCurrent, Position = positionPrevious });
                    }
                }
                while (!(positionStart == positionPrevious && directionCurrent == directionFinish));
            }
            return border;
        }
    }
}
