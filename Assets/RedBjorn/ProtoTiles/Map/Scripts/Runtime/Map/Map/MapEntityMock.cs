using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapEntityMock : IMapNode
    {
        public Dictionary<Vector3Int, TileEntity> Tiles = new Dictionary<Vector3Int, TileEntity>();
        public Func<Vector3Int, Vector3Int, float> DistanceFunc;
        public Vector3Int[] NeighboursDirection { get; set; }

        public float Distance(INode x, INode y)
        {
            return x == null || y == null ? float.MaxValue : DistanceFunc(x.Position, y.Position);
        }


        public IEnumerable<INode> Neighbours(INode node)
        {
            for (int i = 0; i < NeighboursDirection.Length; i++)
            {
                var n = NeighboursDirection[i];
                yield return Tiles.TryGetOrDefault(n + node.Position);
            }
        }

        public IEnumerable<INode> NeighborsMovable(INode node)
        {
            var nodeEntity = Tiles.TryGetOrDefault(node.Position);
            for (int i = 0; i < NeighboursDirection.Length; i++)
            {
                if (nodeEntity.NeighbourMovable[i] <= 0f)
                {
                    var n = NeighboursDirection[i];
                    var neigh = Tiles.TryGetOrDefault(n + node.Position);
                    if (neigh != null)
                    {
                        yield return neigh;
                    }
                }
            }
        }

        public void Reset()
        {
            foreach (var tile in Tiles)
            {
                tile.Value.Depth = float.MaxValue;
                tile.Value.Visited = false;
                tile.Value.Considered = false;
            }
        }

        public void Reset(float range, INode startNode)
        {
            var rangeBorder = Mathf.CeilToInt(range);
            var startPos = startNode.Position;
            for (int x = -rangeBorder; x <= rangeBorder; x++)
            {
                var start = Mathf.Max(-rangeBorder, -x - rangeBorder);
                var finish = Mathf.Min(rangeBorder, -x + rangeBorder);
                for (int y = start; y <= finish; y++)
                {
                    var tile = Tiles.TryGetOrDefault(new Vector3Int(x, y, -x - y) + startPos);
                    if (tile != null)
                    {
                        tile.Depth = float.MaxValue;
                        tile.Visited = false;
                        tile.Considered = false;
                    }
                }
            }
        }
    }
}