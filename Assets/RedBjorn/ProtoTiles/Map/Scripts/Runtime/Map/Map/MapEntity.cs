using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [Serializable]
    public partial class MapEntity : IMapNode, IMapDirectional
    {
        float TileSize = 1f;
        TileDictionary Tiles = new TileDictionary();
        Vector3 Eps;
        Vector3[] Vertices;
        public Vector3[] VerticesInner;
        MapView View;

        Func<Vector3Int, Vector3Int, float> DistanceFunc;
        Func<Vector3Int, float, List<Vector3Int>> AreaFunc;
        Action<Vector3Int, TileDictionary, float> Reset;
        Func<Vector3, float, Vector3Int> WorldPosToTile;
        Func<Vector3Int, float, Vector3> TilePosToWorld;
        Func<Vector3Int, int> VerticeLeftFunc;
        Func<Vector3Int, int> VerticeRightFunc;

        public RotationType RotationType { get; private set; }
        public float TileDistance { get; private set; }
        public MapSettings Settings { get; private set; }
        public MapRules Rules { get { return Settings.Rules; } }

        MapEntity() { }

        public MapEntity(MapSettings settings, MapView view)
        {
            Settings = settings;
            View = view;
            RotationType = Settings.RotationType;
            VerticeLeftFunc = Settings.VerticeLeft;
            VerticeRightFunc = Settings.VerticeRight;
            WorldPosToTile = Settings.ToTile;
            TilePosToWorld = Settings.ToWorld;
            Tiles = new TileDictionary();
            for (int i = 0; i < settings.Tiles.Count; i++)
            {
                var tilePreset = settings.Tiles[i];
                var type = settings.Presets.FirstOrDefault(t => t.Id == tilePreset.Id);
                Tiles[tilePreset.TilePos] = new TileEntity(tilePreset, type, Rules);
            }
            if (settings.Type == GridType.HexFlat)
            {
                TileSize = settings.Edge;
                TileDistance = TileSize * Hex.DistanceBetweenCenters;
                DistanceFunc = Hex.Distance;
                NeighboursDirection = Hex.Neighbour;
                AreaFunc = Hex.Area;
                Vertices = HexFlat.Vertices(Settings.Axis);
                Reset = ResetHex;
                Eps = Hex.Eps;
            }
            else if (settings.Type == GridType.HexPointy)
            {
                TileSize = settings.Edge;
                TileDistance = TileSize * Hex.DistanceBetweenCenters;
                DistanceFunc = Hex.Distance;
                NeighboursDirection = Hex.Neighbour;
                AreaFunc = Hex.Area;
                Vertices = HexPointy.Vertices(Settings.Axis);
                Reset = ResetHex;
                Eps = Hex.Eps;
            }
            else if (settings.Type == GridType.Square)
            {
                TileSize = settings.Edge;
                TileDistance = TileSize * settings.Edge;
                DistanceFunc = Square.Distance;
                NeighboursDirection = Square.Neighbour;
                AreaFunc = Square.Area;
                Vertices = Square.Vertices(Settings.Axis);
                Reset = ResetSquare;
                Eps = Square.Eps;
            }

            var offset = TileSize / 100f;
            VerticesInner = new Vector3[Vertices.Length];
            for (int i = 0; i < VerticesInner.Length; i++)
            {
                var inner = Vertices[i];
                var main = Settings.AxisMainGet(inner);
                if (main > 0)
                {
                    Settings.AxisMainSet(inner, main - offset);
                }
                else if (main < 0)
                {
                    Settings.AxisMainSet(inner, main + offset);
                }

                var secondary = Settings.AxisSecondaryGet(inner);
                if (secondary > 0)
                {
                    Settings.AxisSecondarySet(inner, secondary - offset);
                }
                else if (secondary < 0)
                {
                    Settings.AxisSecondarySet(inner, secondary + offset);
                }
                VerticesInner[i] = inner;
            }
        }

        //sunhe added for test
        public TileDictionary GetTileDic()
        {
            return Tiles;
        }
        /// <summary>
        /// Get tile entity by world space position
        /// </summary>
        /// <param name="worldPos">world space position </param>
        /// <returns></returns>
        public TileEntity Tile(Vector3 worldPos)
        {
            return Tiles.TryGetOrDefault(WorldPosToTile(worldPos, TileSize));
        }

        /// <summary>
        /// Get tile entity by position in tile coordinates
        /// </summary>
        /// <param name="tilePos"></param>
        /// <returns></returns>
        public TileEntity Tile(Vector3Int tilePos)
        {
            return Tiles.TryGetOrDefault(tilePos);
        }

        /// <summary>
        /// Get tiles that intersect the segment between 2 points
        /// </summary>
        /// <param name="worldPosStart"></param>
        /// <param name="worldPosFinish"></param>
        /// <param name="maxDistance"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        public List<TileEntity> LineCast(Vector3 worldPosStart,
                                         Vector3 worldPosFinish,
                                         float maxDistance,
                                         Func<TileEntity, bool> valid)
        {
            var result = new List<TileEntity>();
            LineCastNonAlloc(worldPosStart, worldPosFinish, maxDistance, valid, result);
            return result;
        }

        /// <summary>
        /// Get tiles that intersect the segment between 2 points without allocations
        /// </summary>
        /// <param name="worldPosStart"></param>
        /// <param name="worldPosFinish"></param>
        /// <param name="maxDistance"></param>
        /// <param name="valid"></param>
        /// <param name="result"></param>
        public void LineCastNonAlloc(Vector3 worldPosStart,
                                     Vector3 worldPosFinish,
                                     float maxDistance,
                                     Func<TileEntity, bool> valid,
                                     List<TileEntity> result)
        {
            if (result == null)
            {
                Log.E("LineCastNonAlloc early return. Method can't work with null referene at result");
                return;
            }
            result.Clear();
            var distance = Distance(worldPosStart, worldPosFinish);
            if (distance < 0.01f)
            {
                var tile = Tile(worldPosStart);
                if (tile != null)
                {
                    result.Add(tile);
                }
                return;
            }
            var step = (worldPosFinish - worldPosStart) / distance;
            distance = Mathf.Min(distance, maxDistance);
            var delta = 0f;
            var run = true;
            TileEntity current = null;
            while (run)
            {
                if (delta > distance)
                {
                    delta = distance;
                    run = false;
                }
                var tile = Tile(worldPosStart + step * delta);
                if (tile == null || Distance(worldPosStart, WorldPosition(tile)) > maxDistance)
                {
                    break;
                }
                if (tile != current)
                {
                    current = tile;
                    result.Add(tile);
                    if (!valid(tile))
                    {
                        break;
                    }
                }
                delta += TileSize / 2f;
            }
        }

        /// <summary>
        /// Get world space position of center of tile entity
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public Vector3 WorldPosition(TileEntity tile)
        {
            return tile == null ? Vector3.zero : TilePosToWorld(tile.Position, TileSize);
        }

        /// <summary>
        /// Get world space position of center of tile bt it's coordinates
        /// </summary>
        /// <param name="tilePos"></param>
        /// <returns></returns>
        public Vector3 WorldPosition(Vector3Int tilePos)
        {
            return TilePosToWorld(tilePos, TileSize);
        }

        /// <summary>
        /// Get world space position of center of tile which is located at world space position
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector3 TileCenter(Vector3 worldPos)
        {
            return TilePosToWorld(WorldPosToTile(worldPos, TileSize), TileSize);
        }

        /// <summary>
        /// Normalize vector scaled to tile distance
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector3 Normalize(Vector3 direction)
        {
            return direction.normalized * TileDistance;
        }

        /// <summary>
        /// Distance between two tiles
        /// </summary>
        /// <param name="tileA"></param>
        /// <param name="tileB"></param>
        /// <returns></returns>
        public float Distance(TileEntity tileA, TileEntity tileB)
        {
            return tileA == null || tileB == null ? float.MaxValue : DistanceFunc(tileA.Position, tileB.Position);
        }

        /// <summary>
        /// Distance between two tiles located at corresponding world space positions
        /// </summary>
        /// <param name="worldPosA"></param>
        /// <param name="worldPosB"></param>
        /// <returns></returns>
        public float Distance(Vector3 worldPosA, Vector3 worldPosB)
        {
            return DistanceFunc(WorldPosToTile(worldPosA, TileSize), WorldPosToTile(worldPosB, TileSize));
        }

        /// <summary>
        /// Get walkable tiles around origin at range maximum
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public HashSet<TileEntity> WalkableTiles(Vector3Int origin, float range)
        {
            var nodes = NodePathFinder.WalkableArea(this, Tile(origin), range);
            var tiles = new HashSet<TileEntity>();
            foreach (var n in nodes)
            {
                tiles.Add(n as TileEntity);
            }
            return tiles;
        }

        /// <summary>
        /// Check if two positions are inside same tile
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        public bool IsSameTile(Vector3 position1, Vector3 position2)
        {
            var tile1 = Tile(position1);
            var tile2 = Tile(position2);
            return tile1 != null && tile2 != null && tile1 == tile2;
        }

        /// <summary>
        /// Check if point is inside tile
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsSameTile(TileEntity tile, Vector3 point)
        {
            var tile2 = Tile(point);
            return tile != null && tile2 != null && tile == tile2;
        }

        /// <summary>
        /// Get first neighbour tile to position which is met defined condtion
        /// </summary>
        /// <param name="position"></param>
        /// <param name="nearest"></param>
        /// <param name="condition"></param>
        /// <param name="orderBy">order of neighbours</param>
        /// <returns></returns>
        public bool NearestPosition(Vector3Int position, out Vector3Int nearest, Func<TileEntity, bool> condition, Func<Vector3Int, float> orderBy)
        {
            nearest = Vector3Int.zero;
            foreach (var n in NeighboursDirection.OrderBy(orderBy))
            {
                var pos = n + position;
                var tile = Tile(pos);
                if (tile != null && condition(tile))
                {
                    nearest = tile.Position;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get path that consist of tile entities
        /// </summary>
        /// <param name="from">start position</param>
        /// <param name="to">finish position</param>
        /// <param name="range">maximum range</param>
        /// <returns></returns>
        public List<TileEntity> PathTiles(Vector3 from, Vector3 to, float range)
        {
            var nodes = NodePathFinder.Path(this, Tile(from), Tile(to), range);
            var path = new List<TileEntity>();
            if (nodes != null)
            {
                foreach (var n in nodes)
                {
                    path.Add(n as TileEntity);
                }
            }
            return path;
        }

        /// <summary>
        /// Get path that consist of world space positions
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Vector3> PathPoints(Vector3 from, Vector3 to, float range)
        {
            var nodes = NodePathFinder.Path(this, Tile(from), Tile(to), range);
            var path = new List<Vector3>();
            if (nodes != null)
            {
                foreach (var n in nodes)
                {
                    path.Add(WorldPosition(n.Position));
                }
            }
            return path;
        }

        /// <summary>
        /// Get tile positions which cross the line between two world space postions
        /// </summary>
        /// <param name="worldPosA"></param>
        /// <param name="worldPosB"></param>
        /// <returns>in tile coordinates</returns>
        public HashSet<Vector3Int> LineTilePositions(Vector3 worldPosA, Vector3 worldPosB)
        {
            worldPosA += Eps;
            worldPosB += Eps;
            var distance = Distance(worldPosA, worldPosB);
            var step = (worldPosB - worldPosA) / distance;
            var result = new HashSet<Vector3Int>();
            for (int i = 0; i <= distance; i++)
            {
                var possible = Tile(worldPosA + step * i);
                if (possible != null)
                {
                    result.Add(possible.Position);
                }
            }
            return result;
        }

        /// <summary>
        /// Get positions of border of walkable area
        /// </summary>
        /// <param name="tilePosition"></param>
        /// <param name="range"></param>
        /// <returns>in world space coordinates</returns>
        public List<Vector3> WalkableBorder(Vector3Int tilePosition, float range)
        {
            var origin = Tile(tilePosition);
            return WalkableBorder(origin, range);
        }

        /// <summary>
        /// Get positions of border of walkable area
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Vector3> WalkableBorder(Vector3 worldPosition, float range)
        {
            var origin = Tile(worldPosition);
            return WalkableBorder(origin, range);
        }

        /// <summary>
        /// Get positions of border of walkable area
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Vector3> WalkableBorder(TileEntity origin, float range)
        {
            var borderPoints = new List<Vector3>();
            var walkable = NodePathFinder.WalkableAreaPositions(this, origin, range);
            var borderedAreas = MapBorder.FindBorderPositions(this, walkable);
            foreach (var point in borderedAreas)
            {
                borderPoints.Add(WorldPosition(point.TilePos) + Vertices[point.VerticeIndex] * TileSize);
            }
            return borderPoints;
        }

        /// <summary>
        /// Get tile positions around origin at max range
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Vector3Int> Area(Vector3Int origin, float range)
        {
            return AreaFunc(origin, range);
        }

        /// <summary>
        /// Get tile positions of existed tiles around origin at max range 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public IEnumerable<Vector3Int> AreaExisted(Vector3Int origin, float range)
        {
            return Area(origin, range).Where(a => Tile(a) != null);
        }

        /// <summary>
        /// Get area world space positions around origin at max range
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Vector3> AreaPositions(Vector3Int origin, float range)
        {
            return Area(origin, range).Select(a => WorldPosition(a)).ToList();
        }

        /// <summary>
        /// Get area world space positions of existed tiles around origin at max range
        /// </summary>
        /// <param name="origin">Origin tile</param>
        /// <param name="range"></param>
        /// <returns>Positions in the space of world coordinates</returns>
        public List<Vector3> AreaExistedPositions(TileEntity origin, float range)
        {
            if (origin == null)
            {
                return null;
            }
            var tilePos = origin.Position;
            return AreaExistedPositions(tilePos, range);
        }

        /// <summary>
        /// Get area world space positions of existed tiles around origin at max range
        /// </summary>
        /// <param name="origin">World position</param>
        /// <param name="range"></param>
        /// <returns>Positions in the space of world coordinates</returns>
        public List<Vector3> AreaExistedPositions(Vector3 origin, float range)
        {
            var tilePos = WorldPosToTile(origin, TileSize);
            return AreaExistedPositions(tilePos, range);
        }

        /// <summary>
        /// Get area world space positions of existed tiles around origin at max range
        /// </summary>
        /// <param name="origin">Tile position</param>
        /// <param name="range"></param>
        /// <returns>Positions in the space of world coordinates</returns>
        public List<Vector3> AreaExistedPositions(Vector3Int origin, float range)
        {
            return AreaExisted(origin, range).Select(a => WorldPosition(a)).ToList();
        }

        /// <summary>
        /// Create grid with standard border size
        /// </summary>
        /// <param name="parent"></param>
        public void CreateGrid(Transform parent)
        {
            var tilePrefab = TileCreate(false, true);
            tilePrefab.transform.SetParent(parent);
            tilePrefab.transform.localPosition = Vector3.zero;
            foreach (var tile in Tiles)
            {
                var worldPos = WorldPosition(tile.Key);
                worldPos = Settings.Projection(worldPos, tile.Value.Preset.GridOffset);
                var go = Spawner.Spawn(tilePrefab, worldPos, Quaternion.identity, parent);
                go.name = $"Tile {tile.Key}";
            }
            tilePrefab.SetActive(false);
        }

        /// <summary>
        /// Create single tile
        /// </summary>
        /// <param name="showInner">show inside part of tile</param>
        /// <param name="showBorder">show border part of tile</param>
        /// <param name="borderSize">border width</param>
        /// <param name="inner">material of inside part</param>
        /// <param name="border">material of border part</param>
        /// <returns></returns>
        public GameObject TileCreate(bool showInner, bool showBorder, float borderSize, Material inner = null, Material border = null)
        {
            return Settings.TileCreate(showInner, inner, showBorder, borderSize, border);
        }

        /// <summary>
        /// Create single tile with standard border size
        /// </summary>
        /// <param name="showInner">show inside part of tile</param>
        /// <param name="showBorder">show border part of tile</param>
        /// <param name="inner">material of inside part</param>
        /// <param name="border">material of border part</param>
        /// <returns></returns>
        public GameObject TileCreate(bool showInner, bool showBorder, Material inner = null, Material border = null)
        {
            return Settings.TileCreate(showInner, inner, showBorder, border);
        }

        /// <summary>
        /// Create single tile from the config
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public GameObject TileCreate(MapSettings.TileVisual config)
        {
            return Settings.TileCreate(config);
        }

        /// <summary>
        /// Set state of gameobject which contains map grid
        /// </summary>
        /// <param name="enable"></param>
        public void GridEnable(bool enable)
        {
            if (View)
            {
                View.GridEnable(enable);
            }
            else
            {
                Log.E($"Can't enable Grid state: {enable}. MapView was not set inside MapEntity");
            }
        }

        /// <summary>
        /// Toggle gameobject which contains map grid
        /// </summary>
        public void GridToggle()
        {
            if (View)
            {
                View.GridToggle();
            }
            else
            {
                Log.E("Can't toggle Grid state. MapView was not set inside MapEntity");
            }
        }

        void ResetSquare(Vector3Int startPos, TileDictionary Tiles, float range)
        {
            var rangeBorder = Mathf.CeilToInt(range);
            for (int x = -rangeBorder; x <= rangeBorder; x++)
            {
                for (int z = -rangeBorder; z <= rangeBorder; z++)
                {
                    var tile = Tiles.TryGetOrDefault(new Vector3Int(x, 0, z) + startPos);
                    if (tile != null)
                    {
                        tile.Depth = float.MaxValue;
                        tile.Visited = false;
                        tile.Considered = false;
                    }
                }
            }
        }

        void ResetHex(Vector3Int startPos, TileDictionary Tiles, float range)
        {
            var rangeBorder = Mathf.CeilToInt(range);
            for (int x = -rangeBorder; x <= rangeBorder; x++)
            {
                var start = Mathf.Max(-rangeBorder, -x - rangeBorder);
                var finish = Mathf.Min(rangeBorder, -x + rangeBorder);
                for (int y = start; y <= finish; y++)
                {
                    var tile = Tiles.TryGetOrDefault(new Vector3Int(x, y, -x - y) + startPos);
                    if (tile != null)
                    {
                        tile.Depth = float.MaxValue;
                        tile.Visited = false;
                        tile.Considered = false;
                    }
                }
            }
        }

        #region IMapNode
        public Vector3Int[] NeighboursDirection { get; private set; }

        float IMapNode.Distance(INode x, INode y)
        {
            return x == null || y == null ? float.MaxValue : DistanceFunc(x.Position, y.Position);
        }

        IEnumerable<INode> IMapNode.Neighbours(INode node)
        {
            for (int i = 0; i < NeighboursDirection.Length; i++)
            {
                var n = NeighboursDirection[i];
                yield return Tiles.TryGetOrDefault(n + node.Position);
            }
        }

        IEnumerable<INode> IMapNode.NeighborsMovable(INode node)
        {
            var nodeEntity = Tiles.TryGetOrDefault(node.Position);
            for (int i = 0; i < NeighboursDirection.Length; i++)
            {
                if (nodeEntity.NeighbourMovable[i] <= 0f)
                {
                    var n = NeighboursDirection[i];
                    var neigh = Tiles.TryGetOrDefault(n + node.Position);
                    if (neigh != null)
                    {
                        yield return neigh;
                    }
                }
            }
        }

        void IMapNode.Reset()
        {
            foreach (var tile in Tiles)
            {
                tile.Value.Depth = float.MaxValue;
                tile.Value.Visited = false;
                tile.Value.Considered = false;
            }
        }

        void IMapNode.Reset(float range, INode startNode)
        {
            if (startNode != null)
            {
                var startPos = startNode.Position;
                Reset(startPos, Tiles, range);
            }
        }
        #endregion // IMapNode

        #region IMapDirectional
        Vector3Int IMapDirectional.TurnLeft(Vector3Int fromDirection)
        {
            var ind = Array.IndexOf(NeighboursDirection, fromDirection * -1);
            return NeighboursDirection[(ind + 1) % NeighboursDirection.Length];
        }

        Vector3Int IMapDirectional.TurnRight(Vector3Int fromDirection)
        {
            var ind = Array.IndexOf(NeighboursDirection, fromDirection * -1);
            if (ind == 0)
            {
                return NeighboursDirection[NeighboursDirection.Length - 1];
            }
            return NeighboursDirection[ind - 1];
        }

        int IMapDirectional.VerticeLeft(Vector3Int direction)
        {
            return VerticeLeftFunc(direction);
        }

        int IMapDirectional.VerticeRight(Vector3Int direction)
        {
            return VerticeRightFunc(direction);
        }
        #endregion // IMapDirectional
    }
}
