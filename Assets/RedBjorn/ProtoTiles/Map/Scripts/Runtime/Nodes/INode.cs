using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public interface INode
    {
        Vector3Int Position { get; }
        int MovableArea { get; }
        void ChangeMovableAreaPreset(int area);

        bool Vacant { get; }

        float Depth { get; set; }
        bool Visited { get; set; }
        bool Considered { get; set; }
    }
}