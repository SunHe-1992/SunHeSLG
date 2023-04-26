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
        public float topHeight = 0f;
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
        PawnCamp _camp;
        public PawnCamp camp { get => _camp; set => _camp = value; }

        TileEntity() { }

        public TileEntity(Vector3Int pos, int _tileId, float height)
        {
            TilePos = pos;
            this.tileId = _tileId;
            this.topHeight = height;
        }

        public override string ToString()
        {
            return string.Format($"tileId = {this.tileId} Position: {Position}");
        }

        public void ChangeMovableAreaPreset(int area)
        {

        }

        public string name;
    }

}
