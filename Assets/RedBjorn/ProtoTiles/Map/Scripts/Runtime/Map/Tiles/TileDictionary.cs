using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public class TileDictionary : SerializableDictionary<Vector3Int, TileEntity> { } // Hack to serialize dictionary
}