using System;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public partial class TileEntity : INode
    {
        int CachedMovabeArea;
        int ObstacleCount;
        public TileData Data { get; private set; }
        public TilePreset Preset { get; private set; }
        MapRules Rules;

        public int MovableArea { get { return CachedMovabeArea; } set { CachedMovabeArea = value; } }
        public bool Vacant
        {
            get
            {
                if (Data == null || Rules == null || Rules.IsMovable == null)
                {
                    return true;
                }
                return Rules.IsMovable.IsMet(this) && ObstacleCount == 0;
            }
        }

        public bool Visited { get; set; }
        public bool Considered { get; set; }
        public float Depth { get; set; }
        public float[] NeighbourMovable { get { return Data == null ? null : Data.SideHeight; } }
        public Vector3Int Position { get { return Data == null ? Vector3Int.zero : Data.TilePos; } }

        TileEntity() { }

        public TileEntity(TileData preset, TilePreset type, MapRules rules)
        {
            Data = preset;
            Rules = rules;
            Preset = type;
            MovableArea = Data.MovableArea;
        }

        public override string ToString()
        {
            return string.Format("Position: {0}. Vacant = {1}", Position, Vacant);
        }

        public void ChangeMovableAreaPreset(int area)
        {
            Data.MovableArea = area;
        }
    }
}

