using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{
    /// <summary>
    /// data format for saving to json
    /// </summary>
    public class TileData4Json
    {
        public int pType = 0;
        public int cost = 1;
        public int effect = 0;
        public string name = "";
        public int x;
        public int y;
    }

    //public class BattleMapData
    //{
    //    public int MapID;
    //    public int MapRows;
    //    public int MapCols;

    //}

    #region node interfaces
    public interface INode
    {
        public Vector3Int Position { get; }
        int MovableArea { get; }
        void ChangeMovableAreaPreset(int area);

        bool Vacant { get; }

        int Depth { get; set; }
        bool Visited { get; set; }
        bool Considered { get; set; }
    }
    public interface IMapNode
    {
        int Distance(INode x, INode y, bool extraPrice);
        IEnumerable<INode> Neighbours(INode node);
        IEnumerable<INode> NeighborsMovable(INode node);
        void Reset();
        void Reset(int range, INode startNode);
    }
    #endregion
}
