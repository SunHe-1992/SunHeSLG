using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SunHeTBS
{

    public class NodePathFinder
    {
        static Dictionary<PawnCamp, bool[]> campPassDic = new Dictionary<PawnCamp, bool[]>()
        {
            //if a tile is default ,it is empty
            /*                            defalut player villian ally   neutral */
            { PawnCamp.Default,   new bool[]{true, true , true , true , true } },
            { PawnCamp.Player,    new bool[]{true, true , false, true , false} },
            { PawnCamp.Villain,   new bool[]{true, false, true, false, false } },
            { PawnCamp.PlayerAlly,new bool[]{true, true , false ,true , false } },
            { PawnCamp.Neutral,   new bool[]{true, false ,false, false, true } },
        };
        static bool IsCampPassable(PawnCamp campA, PawnCamp campB, bool passFoe)
        {
            //if unit has skill [Pass] foes do not block this unit's movement 
            if (passFoe)
                return true;
            else
                return campPassDic[campA][(int)campB];
        }

        public static HashSet<INode> WalkableArea(IMapNode map, INode origin, int movePoints, bool extraPrice, bool passFoe, bool isFlier)
        {
            /*ground units spend extra move price on some tiles, however fliers do not*/
            map.Reset(Mathf.CeilToInt(movePoints), origin);
            origin.Depth = 0;
            var open = new Queue<INode>();
            var closed = new HashSet<INode>();

            open.Enqueue(origin);
            var index = 0;
            PawnCamp originCamp = origin.camp;
            while (open.Count > 0 && index < 100000)
            {
                var current = open.Dequeue();
                current.Considered = true;
                foreach (INode n in map.NeighborsMovable(current, isFlier).Where(neigh => neigh != null))
                {
                    int currentDistance = current.Depth + map.Distance(current, n, extraPrice);
                    if (IsCampPassable(originCamp, n.camp, passFoe) && n.Vacant && !n.Considered && currentDistance <= movePoints)
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


        #region path find

        static Dictionary<INode, float> ScoreG = new Dictionary<INode, float>();
        static Dictionary<INode, float> ScoreF = new Dictionary<INode, float>();
        static Dictionary<INode, INode> CameFrom = new Dictionary<INode, INode>();

        public static List<INode> Path(IMapNode map, INode from, INode to, bool extraPrice, bool isPassFoe, bool isFlier)
        {
            return FindPath(map, from, to, extraPrice, isPassFoe, isFlier);
        }
        static List<INode> FindPath(IMapNode map, INode start, INode finish, bool extraPrice, bool isPassFoe, bool isFlier)
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
            ScoreF[start] = map.Distance(start, finish, extraPrice);
            ScoreG[start] = 0;
            PawnCamp startCamp = start.camp;

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
                var list = map.NeighborsMovable(check, isFlier).Where(n => n.Vacant);
                foreach (var node in list)
                {
                    bool campPassable = IsCampPassable(startCamp, node.camp, isPassFoe);
                    if (campPassable == false)
                        continue;
                    var currengScoreG = ScoreG[check] + map.Distance(node, finish, false);
                    var gN = -1f;
                    if (ScoreG.TryGetValue(node, out gN))
                    {
                        if (currengScoreG < gN)
                        {
                            CameFrom[node] = check;
                            ScoreG[node] = currengScoreG;
                            ScoreF[node] = currengScoreG + map.Distance(node, finish, false);
                            CameFrom[node] = check;
                        }
                    }
                    else
                    {
                        open.Add(node);
                        ScoreG[node] = currengScoreG;
                        ScoreF[node] = currengScoreG + map.Distance(node, finish, false);
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

        #endregion
    }

}
