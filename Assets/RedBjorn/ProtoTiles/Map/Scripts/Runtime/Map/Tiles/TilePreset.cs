using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public class TilePreset
    {
        public string Type;
        public string Id;
        public Color MapColor;
        public GameObject Prefab;
        public float GridOffset;
        public List<TileTag> Tags = new List<TileTag>();
    }
}