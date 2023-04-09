using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunHeTBS
{
    /*save info of a tile,*/
    public class TileEntity : INode
    {
        int CachedMovabeArea;
        Vector3Int TilePos;
        public int tileId;
        public TilePassType passType = TilePassType.Passable;
        public EffectType effectType = EffectType.None;
        public int extraPassPrice = 0;
        /// <summary>
        /// standing pawn id
        /// </summary>
        public int pawnId = 0;
        public int MovableArea { get { return CachedMovabeArea; } set { CachedMovabeArea = value; } }
        public bool Vacant
        {
            get
            {
                return true;
            }
        }


        public bool Visited { get; set; }
        public bool Considered { get; set; }
        public int Depth { get; set; }
        public Vector3Int Position { get { return TilePos; } }
        public HashSet<int> rangeHash = new HashSet<int>();
        PawnCamp _camp;
        public PawnCamp camp { get => _camp; set => _camp = value; }

        TileEntity() { }

        public TileEntity(Vector3Int pos, int _tileId)
        {
            TilePos = pos;
            this.tileId = _tileId;
            rangeHash = new HashSet<int>();
        }

        public override string ToString()
        {
            return string.Format("Position: {0}. Vacant = {1}", Position, Vacant);
        }

        public void ChangeMovableAreaPreset(int area)
        {

        }
        public bool ContainsRange(int rangeMin, int rangeMax)
        {
            if (this.rangeHash != null && rangeHash.Count > 0)
            {
                for (int i = rangeMin; i <= rangeMax; i++)
                {
                    if (rangeHash.Contains(i))
                        return true;
                }
            }
            return false;
        }
    }

}
