using System.Collections.Generic;

namespace RedBjorn.ProtoTiles
{
    public interface IMapNode
    {
        float Distance(INode x, INode y);
        IEnumerable<INode> Neighbours(INode node);
        IEnumerable<INode> NeighborsMovable(INode node);
        void Reset();
        void Reset(float range, INode startNode);
    }
}