using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [CreateAssetMenu(menuName = "RedBjorn/ProtoTiles/Map")]
    public class MapSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class TileVisual
        {
            public float BorderSize;
            public bool ShowInner;
            public Material Inner;
            public bool ShowBorder;
            public Material Border;
        }

        public GridType Type;
        public GridAxis Axis;
        public RotationType RotationType;
        [HideInInspector] public float Edge = 1f;
        [HideInInspector] public LayerMask MapMask;
        [Range(0f, 1f)] public float BorderSize = 0.025f;
        public Material CellMaterial;
        public MapRules Rules;
        public List<TilePreset> Presets = new List<TilePreset>();
        public List<TileData> Tiles = new List<TileData>();

#pragma warning disable 0414
        string DefaultTileTypeName = "Ground";
#pragma warning restore 0414

        /// <summary>
        /// Method converting position in world space coordinates to tile coordinates
        /// </summary>
        public Vector3Int ToTile(Vector3 point, float size)
        {
            switch (Type)
            {
                case GridType.HexFlat: return HexFlat.ToHexagonal(point, Axis, size);
                case GridType.HexPointy: return HexPointy.ToHexagonal(point, Axis, size);
                case GridType.Square: return Square.ToSquare(point, Axis, size);
            }
            return Vector3Int.zero;
        }

        /// <summary>
        /// Method converting position in tile coordinates to world space coordinates
        /// </summary>
        public Vector3 ToWorld(Vector3Int point, float size)
        {
            switch (Type)
            {
                case GridType.HexFlat: return HexFlat.ToWorld(point, Axis, size);
                case GridType.HexPointy: return HexPointy.ToWorld(point, Axis, size);
                case GridType.Square: return Square.ToWorld(point, Axis, size);
            }
            return Vector3.zero;
        }

        public Plane Plane()
        {
            switch (Axis)
            {
                case GridAxis.XZ: return new Plane(Vector3.up, Vector3.zero);
                case GridAxis.XY: return new Plane(Vector3.back, Vector3.zero);
                case GridAxis.ZY: return new Plane(Vector3.right, Vector3.zero);
            }
            return new Plane();
        }
        /// <summary>
        /// Method calculating distance between two tiles with corresponding positions with side size = 1
        /// </summary>
        Func<Vector3Int, Vector3Int, float> DistanceFunc
        {
            get
            {
                switch (Type)
                {
                    case GridType.HexFlat: return Hex.Distance;
                    case GridType.HexPointy: return Hex.Distance;
                    case GridType.Square: return Square.Distance;
                }
                return null;
            }
        }

        /// <summary>
        /// Neighbour directions in integer coordinates with side size = 1
        /// </summary>
        Vector3Int[] NeighbourDirections
        {
            get
            {
                switch (Type)
                {
                    case GridType.HexFlat: return Hex.Neighbour;
                    case GridType.HexPointy: return Hex.Neighbour;
                    case GridType.Square: return Square.Neighbour;
                }
                return null;
            }
        }

        /// <summary>
        /// Init map settings asset with new data
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rules"></param>
        public void Init(GridType grid, GridAxis axis, MapRules rules, Material material)
        {
            Type = grid;
            Axis = axis;
            Edge = 1f;
            Rules = rules;
            CellMaterial = material;
            RotationType = axis == GridAxis.XZ ? RotationType.LookAt : RotationType.Flip;
            Presets.Clear();
            Tiles.Clear();
        }

        /// <summary>
        /// Add default preset for map settings
        /// </summary>
        /// <returns></returns>
        public TilePreset PresetAddDefault()
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Add Tile Preset");
            var tile = new TilePreset
            {
                Id = GUID.Generate().ToString(),
                MapColor = new Color(1f, 1f, 1f, 0.5f),
                Type = DefaultTileTypeName
            };
            Presets.Add(tile);
            return tile;
#else
            return null;
#endif
        }

        /// <summary>
        /// Remove preset at index
        /// </summary>
        /// <param name="index"></param>
        public void PresetRemove(int index)
        {
            if (index < 0 || index >= Presets.Count)
            {
                Log.E($"Can't remove tyle preset {index}, count: {Presets.Count}");
                return;
            }
#if UNITY_EDITOR
            Undo.RecordObject(this, "Remove Tile Preset");
#endif
            var currentPreset = Presets[index];
            Presets.RemoveAt(index);
            Tiles.RemoveAll(p => p.Id == currentPreset.Id);
        }

        /// <summary>
        /// Tile side rotation
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float TileSideRotation(int index)
        {
            switch (Type)
            {
                case GridType.HexFlat: return Hex.SideRotation[index];
                case GridType.HexPointy: return Hex.SideRotation[index];
                case GridType.Square: return Square.SideRotation[index];
            }
            return 0f;
        }

        /// <summary>
        /// Find tile data by integer position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public TileData GetTile(Vector3Int position)
        {
            return Tiles.FirstOrDefault(t => t.TilePos == position);
        }

        /// <summary>
        /// Tile center in world space coordinates
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 TileCenterWorld(Vector3 position)
        {
            switch (Type)
            {
                case GridType.HexFlat: return HexFlat.Center(position, Axis, Edge);
                case GridType.HexPointy: return HexPointy.Center(position, Axis, Edge);
                case GridType.Square: return Square.Center(position, Axis, Edge);
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Index of tile neighbour located at direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public int TileNeighbourIndexAtDirection(Vector3 direction)
        {
            switch (Type)
            {
                case GridType.HexFlat: return HexFlat.NeighbourTileIndexAtDirection(direction, Axis);
                case GridType.HexPointy: return HexPointy.NeighbourTileIndexAtDirection(direction, Axis);
                case GridType.Square: return Square.NeighbourTileIndexAtDirection(direction, Axis);
            }
            return 0;
        }

        /// <summary>
        /// Neighbour index of opposite tile neighbour
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int TileNeighbourIndexOpposite(int index)
        {
            switch (Type)
            {
                case GridType.HexFlat: return (index + 3) % Hex.Neighbour.Length;
                case GridType.HexPointy: return (index + 3) % Hex.Neighbour.Length;
                case GridType.Square: return (index + 2) % Square.Neighbour.Length;
            }
            return 0;
        }

        /// <summary>
        /// Neighbour directions in tile coordinates with side size = 1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3Int TileNeighbourAtIndex(int index)
        {
            switch (Type)
            {
                case GridType.HexFlat:
                    if (index >= Hex.Neighbour.Length)
                    {
                        Log.E($"Index = {index}");
                        return Vector3Int.zero;
                    }
                    return Hex.Neighbour[index];
                case GridType.HexPointy:
                    if (index >= Hex.Neighbour.Length)
                    {
                        Log.E($"Index = {index}");
                        return Vector3Int.zero;
                    }
                    return Hex.Neighbour[index];
                case GridType.Square:
                    if (index >= Square.Neighbour.Length)
                    {
                        Log.E($"Index = {index}");
                        return Vector3Int.zero;
                    }
                    return Square.Neighbour[index];
            }
            return Vector3Int.zero;
        }

        public int VerticeLeft(Vector3Int direction)
        {
            switch (Type)
            {
                case GridType.HexFlat: return HexFlat.VerticeIndexLeft(direction);
                case GridType.HexPointy: return HexPointy.VerticeIndexLeft(direction);
                case GridType.Square: return Square.VerticeIndexLeft(direction);
            }
            return 0;
        }

        public int VerticeRight(Vector3Int direction)
        {
            switch (Type)
            {
                case GridType.HexFlat: return HexFlat.VerticeIndexRight(direction);
                case GridType.HexPointy: return HexPointy.VerticeIndexRight(direction);
                case GridType.Square: return Square.VerticeIndexRight(direction);
            }
            return 0;
        }

        /// <summary>
        /// Clear Map View at Scene View and clear tile data
        /// </summary>
        public void Clear()
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Clear");
            Tiles.Clear();
            var holder = FindObjectOfType<MapView>();
            if (holder != null)
            {
                for (int i = holder.transform.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(holder.transform.GetChild(i).gameObject);
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
#endif
        }

        public void MapAnalyze()
        {
            switch (Type)
            {
                case GridType.HexFlat: MapAnalyzeHex(); break;
                case GridType.HexPointy: MapAnalyzeHex(); break;
                case GridType.Square: MapAnalyzeSquare(); break;
            }
        }

        void MapAnalyzeHex()
        {
            //TODO
        }

        void MapAnalyzeSquare()
        {
            //TODO
        }

        public GameObject TileCreate(MapSettings.TileVisual config)
        {
            return TileCreate(config.ShowInner, config.Inner, config.ShowBorder, config.BorderSize, config.Border);
        }

        public GameObject TileCreate(bool showInner, Material inner, bool showBorder, Material border)
        {
            return TileCreate(showInner, inner, showBorder, BorderSize, border);
        }

        public GameObject TileCreate(bool showInner, Material inner, bool showBorder, float borderSize, Material border)
        {
            switch (Type)
            {
                case GridType.HexFlat: return TileCreateHex(HexFlat.Vertices(Axis), Edge, showInner, inner, showBorder, borderSize, border ?? CellMaterial);
                case GridType.HexPointy: return TileCreateHex(HexPointy.Vertices(Axis), Edge, showInner, inner, showBorder, borderSize, border ?? CellMaterial);
                case GridType.Square: return TileCreateSquare(Edge, showInner, inner, showBorder, borderSize, border ?? CellMaterial);
            }
            return null;
        }

        GameObject TileCreateHex(Vector3[] verticesPos, float size, bool showInner, Material innerMaterial, bool showBorder, float border, Material borderMaterial)
        {
            var go = new GameObject("Tile");
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            border = Mathf.Clamp01(border);

            vertices.Add(verticesPos[0] * size);
            vertices.Add(verticesPos[1] * size);
            vertices.Add(verticesPos[2] * size);
            vertices.Add(verticesPos[3] * size);
            vertices.Add(verticesPos[4] * size);
            vertices.Add(verticesPos[5] * size);
            vertices.Add(verticesPos[0] * (1 - border) * size);
            vertices.Add(verticesPos[1] * (1 - border) * size);
            vertices.Add(verticesPos[2] * (1 - border) * size);
            vertices.Add(verticesPos[3] * (1 - border) * size);
            vertices.Add(verticesPos[4] * (1 - border) * size);
            vertices.Add(verticesPos[5] * (1 - border) * size);
            mesh.SetVertices(vertices);

            int submesh = 0;
            var triangles = new List<int>();
            var materials = new List<Material>();
            if (showBorder)
            {
                triangles.Add(0);
                triangles.Add(1);
                triangles.Add(6);
                triangles.Add(1);
                triangles.Add(7);
                triangles.Add(6);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(7);
                triangles.Add(2);
                triangles.Add(8);
                triangles.Add(7);
                triangles.Add(2);
                triangles.Add(3);
                triangles.Add(8);
                triangles.Add(3);
                triangles.Add(9);
                triangles.Add(8);
                triangles.Add(3);
                triangles.Add(4);
                triangles.Add(9);
                triangles.Add(4);
                triangles.Add(10);
                triangles.Add(9);
                triangles.Add(4);
                triangles.Add(5);
                triangles.Add(10);
                triangles.Add(5);
                triangles.Add(11);
                triangles.Add(10);
                triangles.Add(0);
                triangles.Add(11);
                triangles.Add(5);
                triangles.Add(0);
                triangles.Add(6);
                triangles.Add(11);
                mesh.SetTriangles(triangles, submesh);
                materials.Add(borderMaterial);
                submesh++;
            }

            if (showInner)
            {
                mesh.subMeshCount = submesh + 1;
                triangles.Clear();
                triangles.Add(6);
                triangles.Add(7);
                triangles.Add(11);
                triangles.Add(7);
                triangles.Add(8);
                triangles.Add(11);
                triangles.Add(8);
                triangles.Add(9);
                triangles.Add(11);
                triangles.Add(9);
                triangles.Add(10);
                triangles.Add(11);
                mesh.SetTriangles(triangles, submesh);
                materials.Add(innerMaterial);
                submesh++;
            }

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = mesh;
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.sharedMaterials = materials.ToArray();
            return go;
        }

        GameObject TileCreateSquare(float size, bool showInner, Material innerMaterial, bool showBorder, float border, Material borderMaterial)
        {
            var go = new GameObject("Tile");
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            border = Mathf.Clamp01(border);
            var borderHalf = border / 2f;
            var verticesPos = Square.Vertices(Axis);
            vertices.Add(verticesPos[0] * size);
            vertices.Add(verticesPos[1] * size);
            vertices.Add(verticesPos[2] * size);
            vertices.Add(verticesPos[3] * size);
            vertices.Add((verticesPos[0] + VectorCreate(-borderHalf, -borderHalf)) * size);
            vertices.Add((verticesPos[1] + VectorCreate(-borderHalf, borderHalf)) * size);
            vertices.Add((verticesPos[2] + VectorCreate( borderHalf, borderHalf)) * size);
            vertices.Add((verticesPos[3] + VectorCreate( borderHalf, -borderHalf)) * size);
            mesh.SetVertices(vertices);

            int submesh = 0;
            var triangles = new List<int>();
            var materials = new List<Material>();
            if (showBorder)
            {
                triangles.Add(1);
                triangles.Add(4);
                triangles.Add(0);
                triangles.Add(1);
                triangles.Add(5);
                triangles.Add(4);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(5);
                triangles.Add(2);
                triangles.Add(6);
                triangles.Add(5);
                triangles.Add(2);
                triangles.Add(3);
                triangles.Add(6);
                triangles.Add(3);
                triangles.Add(7);
                triangles.Add(6);
                triangles.Add(0);
                triangles.Add(7);
                triangles.Add(3);
                triangles.Add(0);
                triangles.Add(4);
                triangles.Add(7);
                mesh.SetTriangles(triangles, submesh);
                materials.Add(borderMaterial);
                submesh++;
            }

            if (showInner)
            {
                mesh.subMeshCount = submesh + 1;
                triangles.Clear();
                triangles.Add(4);
                triangles.Add(5);
                triangles.Add(7);

                triangles.Add(5);
                triangles.Add(6);
                triangles.Add(7);
                mesh.SetTriangles(triangles, submesh);
                materials.Add(innerMaterial);
            }

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = mesh;
            var renderer = go.AddComponent<MeshRenderer>();
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.sharedMaterials = materials.ToArray();
            return go;
        }

#if UNITY_EDITOR
        public const string Suffix = "Map";

        public static string Path(string directory, string filename, string grid)
        {
            return System.IO.Path.Combine(directory, string.Concat(filename, "_", Suffix, grid, FileFormat.Asset));
        }

        public static MapSettings Create(string directory, 
                                         string filename, 
                                         GridType grid, 
                                         GridAxis axis, 
                                         MapRules rules, 
                                         Material material)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var mapPath = Path(directory, filename, grid.ToString());
            mapPath = AssetDatabase.GenerateUniqueAssetPath(mapPath);
            var mapInstance = ScriptableObject.CreateInstance<MapSettings>();
            mapInstance.Init(grid, axis, rules, material);
            AssetDatabase.CreateAsset(mapInstance, mapPath);
            AssetDatabase.SaveAssets();
            return mapInstance;
        }
#endif
        public float AxisMainGet(Vector3 target)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return target.x;
                case GridAxis.XY: return target.x;
                case GridAxis.ZY: return target.z;
            }
            return 0f;
        }

        public float AxisSecondaryGet(Vector3 target)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return target.z;
                case GridAxis.XY: return target.y;
                case GridAxis.ZY: return target.y;
            }
            return 0f;
        }

        public float AxisOrthogonalGet(Vector3 target)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return target.y;
                case GridAxis.XY: return target.z;
                case GridAxis.ZY: return target.x;
            }
            return 0f;
        }

        public Vector3 AxisMainSet(Vector3 target, float val)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return VectorCreate(val, target.y, target.z);
                case GridAxis.XY: return VectorCreate(val, target.y, target.z);
                case GridAxis.ZY: return VectorCreate(target.x, target.y, val);
            }
            return target;
        }

        public Vector3 AxisSecondarySet(Vector3 target, float val)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return VectorCreate(target.x, target.y, val);
                case GridAxis.XY: return VectorCreate(target.x, val, target.z);
                case GridAxis.ZY: return VectorCreate(target.x, val, target.z);
            }
            return target;
        }

        public Vector3 VectorCreate(float main, float secondary)
        {
            return VectorCreate(main, secondary, 0f);
        }

        public Vector3 VectorCreate(float main, float secondary, float orthogonal)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return new Vector3(main, orthogonal, secondary);
                case GridAxis.XY: return new Vector3(main, secondary, -orthogonal);
                case GridAxis.ZY: return new Vector3(orthogonal, secondary, main);
            }
            return Vector3.zero;
        }

        public Vector3 VectorCreateOrthogonal(float orthogonalValue)
        {
            return VectorCreate(0f, 0f, orthogonalValue);
        }

        public Vector3 Projection(Vector3 target)
        {
            return Projection(target, 0f);
        }

        public Vector3 Projection(Vector3 target, Vector3 offset)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return Projection(target, offset.y);
                case GridAxis.XY: return Projection(target, offset.z);
                case GridAxis.ZY: return Projection(target, offset.x);
            }
            return Vector3.zero;
        }

        public Vector3 Projection(Vector3 target, float offset)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return new Vector3(target.x, offset, target.z);
                case GridAxis.XY: return new Vector3(target.x, target.y, -offset);
                case GridAxis.ZY: return new Vector3(offset, target.y, target.z);
            }
            return Vector3.zero;
        }

        public Vector3 ProjectionOrthogonal(Vector3 target)
        {
            return Projection(Vector3.zero, target);
        }

        public Vector3 ProjectionXY(Vector3 target)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return new Vector3(target.x, target.z, 0f);
                case GridAxis.XY: return new Vector3(target.x, target.y, 0f);
                case GridAxis.ZY: return new Vector3(target.z, target.y, 0f);
            }
            return Vector3.zero;
        }

        public Quaternion LookAt(Vector3 inDirection)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return Quaternion.Euler(0f, Mathf.Atan2(inDirection.x, inDirection.z) * Mathf.Rad2Deg, 0f);
                case GridAxis.XY: return Quaternion.Euler(0f, 0f, Mathf.Atan2(inDirection.y, inDirection.x) * Mathf.Rad2Deg);
                case GridAxis.ZY: return Quaternion.Euler(Mathf.Atan2(inDirection.y, inDirection.z) * Mathf.Rad2Deg, 0f, 0f);
            }
            return Quaternion.identity;
        }

        public Quaternion Flip(Vector3 inDirection)
        {
            var secondary = AxisMainGet(inDirection) > 0f ? 0f : 180f;
            var euler = VectorCreate(0f, secondary);
            return Quaternion.Euler(euler);
        }

        public Quaternion RotationPlane()
        {
            switch (Axis)
            {
                case GridAxis.XZ: return Quaternion.Euler(90f, 0f, 0f);
                case GridAxis.XY: return Quaternion.identity;
                case GridAxis.ZY: return Quaternion.Euler(0f, -90f, 0f);
            }
            return Quaternion.identity;
        }

        public float SignedAngle(Transform trans, Vector3 target)
        {
            switch (Axis)
            {
                case GridAxis.XZ: return Vector3.SignedAngle(trans.forward, target, Vector3.up);
                case GridAxis.XY: return Vector3.SignedAngle(trans.up, target, Vector3.back);
                case GridAxis.ZY: return Vector3.SignedAngle(trans.up, target, Vector3.right);
            }
            return 0f;
        }
    }
}