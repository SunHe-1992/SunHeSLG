using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SunHeTBS
{

    public class NodePathFinder
    {

        public static HashSet<INode> WalkableArea(IMapNode map, INode origin, int range, bool extraPrice = false)
        {
            /*ground units spend extra move price on some tiles, however fliers do not*/
            map.Reset(Mathf.CeilToInt(range), origin);
            origin.Depth = 0;
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
                    var currentDistance = current.Depth + map.Distance(current, n, extraPrice);
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
    }

}
