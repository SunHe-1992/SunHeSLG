using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class NodePathFinder
    {
        static Dictionary<INode, float> ScoreG = new Dictionary<INode, float>();
        static Dictionary<INode, float> ScoreF = new Dictionary<INode, float>();
        static Dictionary<INode, INode> CameFrom = new Dictionary<INode, INode>();

        public static HashSet<INode> AccessibleArea(IMapNode map, INode origin)
        {
            map.Reset();
            var open = new Queue<INode>();
            var closed = new HashSet<INode>();

            open.Enqueue(origin);
            var index = 0;
            while (open.Count > 0 && index < 100000)
            {
                var current = open.Dequeue();
                current.Considered = true;
                foreach (var n in map.NeighborsMovable(current).Where(neigh => neigh != null))
                {
                    if (n.Vacant && !n.Considered)
                    {
                        n.Considered = true;
                        open.Enqueue(n);
                        index++;
                    }
                }
                current.Visited = true;
                closed.Add(current);

            }
            return closed;
        }

        public static HashSet<INode> WalkableArea(IMapNode map, INode origin, float range)
        {
            map.Reset(Mathf.CeilToInt(range), origin);
            origin.Depth = 0f;
            var open = new Queue<INode>();
            var closed = new HashSet<INode>();

            open.Enqueue(origin);
            var index = 0;
            while (open.Count > 0 && index < 100000)
            {
                var current = open.Dequeue();
                current.Considered = true;
                foreach (var n in map.NeighborsMovable(current).Where(neigh => neigh != null))
                {
                    var currentDistance = current.Depth + map.Distance(current, n);
                    if (n.Vacant && !n.Considered && currentDistance <= range)
                    {
                        n.Considered = true;
                        n.Depth = currentDistance;
                        open.Enqueue(n);
                        index++;
                    }
                }
                current.Visited = true;
                closed.Add(current);

            }
            return closed;
        }

        public static HashSet<Vector3Int> WalkableAreaPositions(IMapNode map, INode origin, float range)
        {
            map.Reset(Mathf.CeilToInt(range), origin);
            origin.Depth = 0f;
            var open = new Queue<INode>();
            var closed = new HashSet<Vector3Int>();

            open.Enqueue(origin);
            var index = 0;
            while (open.Count > 0 && index < 100000)
            {
                var current = open.Dequeue();
                current.Considered = true;
                foreach (var n in map.NeighborsMovable(current).Where(neigh => neigh != null))
                {
                    var currentDistance = current.Depth + map.Distance(current, n);
                    if (n.Vacant && !n.Considered && currentDistance <= range)
                    {
                        n.Considered = true;
                        n.Depth = currentDistance;
                        open.Enqueue(n);
                        index++;
                    }
                }
                current.Visited = true;
                closed.Add(current.Position);

            }
            return closed;
        }

        public static List<INode> Path(IMapNode map, INode start, INode finish, float range)
        {
            if (start.MovableArea != finish.MovableArea)
            {
                return null;
            }
            var fullPath = FindPath(map, start, finish);
            return TrimPath(map, fullPath, range);
        }

        public static List<INode> Path(IMapNode map, INode start, INode finish)
        {
            if (start.MovableArea != finish.MovableArea)
            {
                return null;
            }
            return FindPath(map, start, finish);
        }

        static List<INode> FindPath(IMapNode map, INode start, INode finish)
        {
            ScoreG.Clear();
            ScoreF.Clear();
            CameFrom.Clear();

            var path = new List<INode>();
            if (!finish.Vacant)
            {
                return path;
            }
            var open = new List<INode>();
            var closed = new List<INode>();
            open.Add(start);
            ScoreF[start] = map.Distance(start, finish);
            ScoreG[start] = 0;

            while (open.Any())
            {
                var check = open.OrderBy(o => ScoreF[o]).First();
                if (check == finish)
                {
                    break;
                }
                else if (closed.Contains(check))
                {
                    continue;
                }

                closed.Add(check);
                open.Remove(check);
                foreach (var node in map.NeighborsMovable(check).Where(n => n.Vacant))
                {
                    var currengScoreG = ScoreG[check] + map.Distance(node, finish);
                    var gN = -1f;
                    if (ScoreG.TryGetValue(node, out gN))
                    {
                        if (currengScoreG < gN)
                        {
                            CameFrom[node] = check;
                            ScoreG[node] = currengScoreG;
                            ScoreF[node] = currengScoreG + map.Distance(node, finish);
                            CameFrom[node] = check;
                        }
                    }
                    else
                    {
                        open.Add(node);
                        ScoreG[node] = currengScoreG;
                        ScoreF[node] = currengScoreG + map.Distance(node, finish);
                        CameFrom[node] = check;
                    }
                }
            }
            var current = finish;
            while (CameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = CameFrom[current];
            }
            path.Add(start);
            path.Reverse();

            return path;
        }

        static List<INode> TrimPath(IMapNode map, List<INode> path, float range)
        {
            var distance = 0f;
            int trimIndex = -1;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var step = distance + map.Distance(path[i], path[i + 1]);
                if (step <= range)
                {
                    distance = step;
                }
                else
                {
                    trimIndex = i + 1;
                    break;
                }
            }
            if (trimIndex >= 0)
            {
                path.RemoveRange(trimIndex, path.Count - trimIndex);
            }
            return path;
        }
    }
}