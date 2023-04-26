using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunHeTBS
{
    public class MapEntity : IMapNode
    {

        #region IMapNode implement

        public int Distance(INode x, INode y, bool extraPrice)
        {
            return Distance(x as TileEntity, y as TileEntity, extraPrice);
        }
        /// <summary>
        /// find adjacent nodes that exist in map
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<INode> NeighborsMovable(INode node, PawnCamp startCamp, bool isPassFoe, bool isFlier)
        {
            for (int i = 0; i < NeighbourArray.Length; i++)
            {
                var pos = node.Position + NeighbourArray[i];
                int tileId = XY2TileId(pos);
                if (TileDic.ContainsKey(tileId) == false)//filter invalid tileIds
                    continue;
                TileEntity findTile = TileDic[tileId];
                if (findTile.passType == TilePassType.Impassable)//tile is impassable
                    continue;
                if (!isFlier && findTile.passType == TilePassType.FliersOnly)//only flier can move here
                    continue;
                if (!isPassFoe)
                {
                    //check if a foe stands here
                    if (PawnCampTool.CampsHostile(startCamp, findTile.camp))
                        continue;
                }
                yield return findTile;
            }
        }

        public IEnumerable<INode> Neighbors(INode node)
        {
            for (int i = 0; i < NeighbourArray.Length; i++)
            {
                var pos = node.Position + NeighbourArray[i];
                int tileId = XY2TileId(pos);
                if (TileDic.ContainsKey(tileId) == false)//filter invalid tileIds
                    continue;
                TileEntity findTile = TileDic[tileId];
                if (findTile.passType == TilePassType.Impassable)
                    continue;
                yield return findTile;
            }
        }
        public static readonly Vector3Int[] NeighbourArray = new Vector3Int[]
        {
            new Vector3Int(0,1,0),
            new Vector3Int(1,0,0),
            new Vector3Int(0,-1,0),
            new Vector3Int(-1,0,0),
        };
        public IEnumerable<INode> Neighbours(INode node)
        {
            for (int i = 0; i < NeighbourArray.Length; i++)
            {
                var pos = node.Position + NeighbourArray[i];
                int tileId = XY2TileId(pos);
                if (TileDic.ContainsKey(tileId) == false)//filter invalid tileIds
                    continue;
                else
                    yield return TileDic[tileId];
            }
        }

        void IMapNode.Reset()
        {
            foreach (var tile in TileDic.Values)
            {
                tile.Depth = int.MaxValue;
                tile.Visited = false;
                tile.Considered = false;
            }
        }

        public void Reset(int range, INode startNode)
        {
            if (startNode != null)
            {
                ResetSquare(startNode.Position, (int)range);
            }
        }
        #endregion


        public int MapID;
        public int MapRows;
        public int MapCols;
        /// <summary>
        /// unity world size of a tile
        /// </summary>
        public float TileSize = 1f;
        /// <summary>
        /// key=tileId, value=TileEntity
        /// </summary>
        Dictionary<int, TileEntity> TileDic = new Dictionary<int, TileEntity>();
        public int XY2TileId(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return -1;
            }
            return x + y * MapRows;
        }
        public int XY2TileId(Vector3Int vect)
        {
            return XY2TileId(vect.x, vect.y);
        }

        public Vector3Int TileId2XY(int tileId)
        {
            return new Vector3Int(tileId / MapCols, tileId % MapCols);
        }
        public void InitData(int row, int col, List<TileData4Json> tdjList)
        {
            this.MapRows = row;
            this.MapCols = col;
            TileDic = new Dictionary<int, TileEntity>();

            foreach (TileData4Json tdj in tdjList)
            {
                int tileId = XY2TileId(tdj.x, tdj.y);

                TileEntity tile = new TileEntity(new Vector3Int(tdj.x, tdj.y), tileId, tdj.h);
                tile.passType = (TilePassType)tdj.pType;
                tile.extraPassPrice = tdj.cost;
                tile.effectType = (EffectType)tdj.effect;
                TileDic[tileId] = tile;
                tile.name = tdj.name;
            }

        }

        /// <summary>
        /// TileEntity pos -> unity pos
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public Vector3 WorldPosition(TileEntity tile)
        {
            if (tile == null)
                return Vector3.zero;

            return WorldPosition(tile.Position);
        }
        /// <summary>
        /// tile pos -> unity pos
        /// </summary>
        /// <param name="vect"></param>
        /// <returns></returns>
        public Vector3 WorldPosition(Vector3Int vect)
        {
            return new Vector3(vect.x * TileSize, 0f, vect.y * TileSize);
        }
        /// <summary>
        /// unity pos -> tile pos
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector3Int WorldPosToTilePos(Vector3 worldPos)
        {
            return new Vector3Int(Mathf.RoundToInt(worldPos.x / TileSize), Mathf.RoundToInt(worldPos.z / TileSize), 0);
        }
        /// <summary>
        /// unity pos -> tile
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public TileEntity WorldPosition(Vector3 worldPos)
        {
            Vector3Int vect = WorldPosToTilePos(worldPos);
            int tileId = XY2TileId(vect.x, vect.y);
            return GetTileFromDic(tileId);
        }
        /// <summary>
        /// tile id -> tile
        /// </summary>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public TileEntity GetTileFromDic(int tileId)
        {
            if (TileDic.ContainsKey(tileId)) return TileDic[tileId];
            return null;
        }
        /// <summary>
        /// unity pos -> tile
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public TileEntity Tile(Vector3 worldPos)
        {
            return WorldPosition(worldPos);
        }
        /// <summary>
        /// tile pos -> tile
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public TileEntity Tile(Vector3Int pos)
        {
            var tile = GetTileFromDic(XY2TileId(pos));
            //if (tile != null && tile.Position != pos)
            //{
            //    Debugger.LogError("get tile pos is not correct");
            //}
            return tile;
        }
        public TileEntity TileOrDefault(Vector3Int pos)
        {
            var tile = GetTileFromDic(XY2TileId(pos));
            if (tile != null) return tile;
            else return default;
        }
        /// <summary>
        /// tile -> tile pos
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public Vector3 TileCenter(TileEntity tile)
        {
            return WorldPosition(tile);
        }

        /// <summary>
        /// Distance between two tiles located at corresponding world space positions
        /// </summary>
        /// <param name="worldPosA"></param>
        /// <param name="worldPosB"></param>
        /// <returns></returns>
        //public int Distance(Vector3 worldPosA, Vector3 worldPosB, bool extraPrice = false)
        //{
        //    var tileA = WorldPosition(worldPosA);
        //    var tileB = WorldPosition(worldPosB);
        //    return Distance(tileA.Position, tileB.Position, extraPrice); //for fliers no extra pass price
        //}

        /// <summary>
        /// Chessboard distance
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        public int Distance(Vector3Int posA, Vector3Int posB)
        {
            return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        }
        /// <summary>
        /// Distance when calculating ground pawn
        /// </summary>
        /// <param name="tileA"></param>
        /// <param name="tileB"></param>
        /// <returns></returns>
        public int Distance(TileEntity tileA, TileEntity tileB, bool extraPrice)
        {
            if (tileA == null || tileB == null)
                return int.MaxValue;
            return Distance(tileA.Position, tileB.Position) + (extraPrice ? tileB.extraPassPrice : 0);
        }

        /// <summary>
        /// Get walkable tiles around origin at range maximum
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="movePoints"></param>
        /// <returns></returns>
        public HashSet<TileEntity> WalkableTiles(Vector3Int origin, int movePoints, bool extraPrice, bool passFoe, bool isFlier)
        {
            var nodes = NodePathFinder.WalkableArea(this, Tile(origin), movePoints, extraPrice, passFoe, isFlier);
            var tiles = new HashSet<TileEntity>();
            foreach (var n in nodes)
            {
                tiles.Add(n as TileEntity);
            }
            return tiles;
        }


        void ResetSquare(Vector3Int startPos, int range)
        {
            for (int x = -range; x <= range; x++)
                for (int y = -range; y <= range; y++)
                {
                    var tile = GetTileFromDic(XY2TileId(startPos.x + x, startPos.y + y));
                    if (tile != null)
                    {
                        tile.Depth = int.MaxValue;
                        tile.Visited = false;
                        tile.Considered = false;
                    }
                }
        }

        public Vector3Int TrimPos_Border(Vector3Int pos)
        {
            if (pos.x < 0) pos.x = 0;
            if (pos.y < 0) pos.y = 0;
            if (pos.x > MapRows - 1) pos.x = MapRows - 1;
            if (pos.y > MapCols - 1) pos.y = MapCols - 1;
            return pos;
        }

        public Dictionary<int, TileEntity> GetTileDic()
        {
            return TileDic;
        }
    }

}
