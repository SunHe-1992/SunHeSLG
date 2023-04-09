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

        public static HashSet<INode> WalkableArea(IMapNode map, INode origin, int movePoints, bool extraPrice, bool passFoe)
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
                foreach (INode n in map.NeighborsMovable(current).Where(neigh => neigh != null))
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

        /// <summary>
        /// given a tilePos,save range in tiles ,in range of (rangeMin,rangeMax)
        /// </summary>
        /// <param name="rangeMin"></param>
        /// <param name="rangeMax"></param>
        /// <param name="centerPos"></param>
        public static void MarkTileATKRange(int rangeMin, int rangeMax, Vector3Int centerPos)
        {
            // need clear all tiles' rangeHash
            for (int m = -rangeMax; m <= rangeMax; m++)
            {
                int rangeN = rangeMax - Mathf.Abs(m);
                for (int n = -rangeN; n <= rangeN; n++) //m,n loop the diamond shape around centerPos
                {
                    int tileRange = Mathf.Abs(m) + Mathf.Abs(n);
                    if (rangeMin > 0 && tileRange < rangeMin)//consider min range
                    {
                        continue;
                    }
                    // m,n is the pos
                    Vector3Int pos = new Vector3Int(centerPos.x + m, centerPos.y + n, 0);
                    var tile = TBSMapService.Inst.map.Tile(pos);
                    if (tile != null)
                    {
                        tile.rangeHash.Add(tileRange);
                    }
                }
            }
        }
    }

}
