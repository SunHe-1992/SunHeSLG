using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public interface IMapDirectional
    {
        Vector3Int[] NeighboursDirection { get; }
        Vector3Int TurnLeft(Vector3Int fromDirection);
        Vector3Int TurnRight(Vector3Int fromDirection);
        int VerticeLeft(Vector3Int direction);
        int VerticeRight(Vector3Int direction);
    }
}
