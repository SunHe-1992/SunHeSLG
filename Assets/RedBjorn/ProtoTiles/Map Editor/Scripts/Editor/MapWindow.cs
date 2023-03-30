using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class MapWindow : EditorWindowExtended
    {
        [SerializeField]
        int GridAxisType;
        [SerializeField]
        int GridType;
        [SerializeField]
        int RotationType;
        [SerializeField]
        bool ShowMap;
        [SerializeField]
        int TilePresetCurrent;
        [SerializeField]
        int ToolCurrent;

        Rect Common;
        Rect WorkArea;
        Vector2 ScrollPos;
        bool[] ToolToogle = new bool[4];
        bool[] ToolTooglePrevious = new bool[4];
        string[] TileIds;
        MapWindowSettings Settings;

        string ButtonPlacePrefabs = "Place Prefabs";

        static readonly string[] TileToolNames = new string[]
        {
            "Brush",
            "Eraser"
        };

        static readonly string[] EdgeToolNames = new string[]
        {
            "Brush",
            "Eraser"
        };

        static readonly string[] Grids = System.Enum.GetNames(typeof(GridType));
        static readonly string[] GridAxes = System.Enum.GetNames(typeof(GridAxis));
        static readonly string[] Rotations = System.Enum.GetNames(typeof(RotationType));

        [SerializeField]
        MapSceneDrawer CachedSceneDrawer;
        MapSceneDrawer SceneDrawer
        {
            get
            {
                if (CachedSceneDrawer == null)
                {
                    CachedSceneDrawer = new MapSceneDrawer() { Window = this, WindowSettings = Settings };
                }
                return CachedSceneDrawer;
            }
        }

        bool Editable { get { return Map != null; } }
        bool IsSceneEditing { get { return Editable && ShowMap && ToolToogle.Any(t => t); } }
        bool IsSceneEditingPrevious { get { return Editable && ShowMap && ToolTooglePrevious.Any(t => t); } }

        public float WindowWidth;
        public float WindowHeight;
        float CommonHeight => Settings.CommonHeight;
        float Border => Settings.Border;
        float ToolLabelWidth => Settings.ToolLabelWidth;
        float TileLabelWidth => Settings.TileLabelWidth;
        Color Separator => Settings.Separator;
        GUISkin Skin => Settings.Skin;

        [SerializeField]
        MapSettings CachedMap;
        MapSettings Map
        {
            get
            {
                return CachedMap;
            }
            set
            {
                if (CachedMap != value)
                {
                    CachedMap = value;
                    OnChangedMap();
                }
            }
        }

        [MenuItem("Tools/Red Bjorn/Editors/Map")]
        public static void DoShow()
        {
            DoShow(null);
        }

        public static void DoShow(MapSettings map)
        {
            var window = (MapWindow)EditorWindow.GetWindow(typeof(MapWindow));
            window.minSize = MapWindowSettings.WindowMinSize;
            window.titleContent = new GUIContent("Map Editor");
            window.ShowMap = true;
            window.Map = map;
            window.Show();
        }

        void OnEnable()
        {
            InitResources();
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        void OnGUI()
        {
            var scale = EditorGUIUtility.pixelsPerPoint;
            WindowWidth = Screen.width / scale;
            WindowHeight = Screen.height / scale;

            Undo.RecordObject(this, "Map");
            Common.x = 2 * Border;
            Common.y = 2 * Border;
            Common.width = WindowWidth - 4 * Border;
            Common.height = CommonHeight - 2 * Border;

            EditorGUI.DrawRect(Common, Settings.CommonColor);

            GUILayout.BeginArea(Common);
            Map = EditorGUILayout.ObjectField("Map Asset", Map, typeof(MapSettings), allowSceneObjects: false) as MapSettings;
            GuiStyles.DrawHorizontal(Separator);

            EditorGUILayout.LabelField("Map Asset Creator", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Grid", GUILayout.Width(26));
            GridType = GUILayout.SelectionGrid(GridType, Grids, 3, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Axis", GUILayout.Width(26));
            GridAxisType = GUILayout.SelectionGrid(GridAxisType, GridAxes, 3, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Create", GUILayout.MaxWidth(MapWindowSettings.WindowMinSize.x - 6 * Border)))
            {
                MapCreateAsset();
            }

            GuiStyles.DrawHorizontal(Separator);
            GUILayout.EndArea();
            GUI.enabled = Editable;
            WorkArea.x = 2 * Border;
            WorkArea.y = Common.height + 2 * Border;
            WorkArea.width = WindowWidth - 4 * Border;
            WorkArea.height = WindowHeight - CommonHeight - 10 * Border;

            EditorGUI.DrawRect(WorkArea, Settings.WorkAreaColor);

            GUILayout.BeginArea(WorkArea);
            var presetsDetermined = "Not all prefabs at Presets\nare determined.\nContinue?";
            var mapViewChilds = "MapView gameObject has childs.\nThey will be replaced.\nContinue?";
            EditorGUILayout.LabelField("Map", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            ShowMap = EditorGUILayout.Toggle("Show", ShowMap);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Clear", GUILayout.MaxWidth(MapWindowSettings.WindowMinSize.x - 6 * Border)))
            {
                Action yes = () =>
                {
                    Map.Clear();
                    SceneDrawer.Clear();
                };
                ConfirmEditorWindow.Init("Map data and scene\ndata will be cleared.\nContinue?", yesAction: yes);
            }
            if (GUILayout.Button(ButtonPlacePrefabs, GUILayout.MaxWidth(MapWindowSettings.WindowMinSize.x - 6 * Border)))
            {
                var incorrectPrefabs = Map.Presets.Any(p => !p.Prefab);
                Action placeAction = () =>
                {
                    PlacePrefabs();
                };
                if (IsEmpty)
                {
                    if (incorrectPrefabs)
                    {
                        ConfirmEditorWindow.Init(presetsDetermined, yesAction: placeAction);
                    }
                    else
                    {
                        PlacePrefabs();
                    }
                }
                else
                {
                    if (incorrectPrefabs)
                    {
                        Action yesAction = () =>
                        {
                            ConfirmEditorWindow.Init(mapViewChilds, yesAction: placeAction);
                        };
                        ConfirmEditorWindow.Init(presetsDetermined, yesAction: yesAction);
                    }
                    else
                    {
                        ConfirmEditorWindow.Init(mapViewChilds, yesAction: placeAction);
                    }
                }
            }
            GUILayout.Space(10f);
            var guiEnabled = GUI.enabled;
            GUI.enabled = Map;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation", GUILayout.Width(56));
            RotationType = GUILayout.SelectionGrid(RotationType, Rotations, 2, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            if (Map)
            {
                Map.RotationType = (RedBjorn.ProtoTiles.RotationType)RotationType;
            }
            GUI.enabled = guiEnabled;
            GuiStyles.DrawHorizontal(Separator);
            guiEnabled = GUI.enabled;
            GUI.enabled = Editable && ShowMap;
            if (!IsSceneEditingPrevious && IsSceneEditing)
            {
                SceneEditingStart();
            }
            else if (IsSceneEditingPrevious && !IsSceneEditing)
            {
                SceneEditingFinish();
            }

            ToolToogle.CopyTo(ToolTooglePrevious, 0);
            DrawToolTiles();
            if (Settings.DrawSideTool)
            {
                DrawToolEdges();
            }
            GUI.enabled = guiEnabled;

            for (int i = 0; i < ToolToogle.Length; i++)
            {
                if (ToolToogle[i] && ToolToogle[i] != ToolTooglePrevious[i])
                {
                    if (!ToolValidate())
                    {
                        for (int j = 0; j < ToolToogle.Length; j++)
                        {
                            ToolToogle[j] = false;
                        }
                        ConfirmEditorWindow.Init("Presets list is empty.\nPlease, add at least one preset", noText: "OK");
                        break;
                    }
                    ToolCurrent = i;
                    for (int j = 0; j < ToolToogle.Length; j++)
                    {
                        if (j != i)
                        {
                            ToolToogle[j] = false;
                        }
                    }
                    break;
                }
            }
            SceneDrawer.ToolType = ToolCurrent == 0 || ToolCurrent == 2 ? 0 : 1;
            SceneDrawer.BrushType = ToolCurrent < 2 ? 0 : 1;
            if (Map)
            {
                var buttonAddStyle = Skin.customStyles[4];
                var buttonRemoveStyle = Skin.customStyles[5];
                var buttonTypeNormal = Skin.customStyles[7];
                var buttonTypeSelected = Skin.customStyles[8];
                EditorGUILayout.LabelField("Presets", EditorStyles.boldLabel);
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = TileLabelWidth;

                var serializedMap = new SerializedObject(Map);
                var presetsProperty = serializedMap.FindProperty(nameof(Map.Presets));
                for (int i = 0; i < presetsProperty.arraySize; i++)
                {
                    var presetProperty = presetsProperty.GetArrayElementAtIndex(i);
                    var tagsProperty = presetProperty.FindPropertyRelative("Tags");
                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical(GUILayout.Width(30f), GUILayout.MinHeight(100f));

                    if (ButtonTwoStyle(buttonTypeNormal, buttonTypeSelected, TilePresetCurrent == i))
                    {
                        TilePresetCurrent = i;
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();

                    EditorGUILayout.LabelField(string.Format("Type {0}", i));
                    EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative("Type"), new GUIContent("Name"), true);
                    EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative("MapColor"), new GUIContent("Editor Color"), true);
                    EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative("Prefab"), new GUIContent("Prefab"), true);
                    EditorGUILayout.PropertyField(presetProperty.FindPropertyRelative("GridOffset"), new GUIContent("Grid Offset"), true);
                    EditorGUILayout.LabelField("Tags");

                    var previouslabelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 22f;
                    for (int j = 0; j < tagsProperty.arraySize; j++)
                    {
                        GUILayout.BeginHorizontal(GUILayout.Height(20f));
                        var tagProperty = tagsProperty.GetArrayElementAtIndex(j);

                        EditorGUILayout.PropertyField(tagProperty, new GUIContent(" " + j + ":"), true);
                        if (GUILayout.Button("-", buttonRemoveStyle, GUILayout.Width(20f)))
                        {
                            if (tagProperty.objectReferenceValue)
                            {
                                tagProperty.DeleteCommand();
                            }
                            tagProperty.DeleteCommand();
                            GUILayout.EndHorizontal();
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    EditorGUIUtility.labelWidth = previouslabelWidth;
                    if (GUILayout.Button("+", buttonAddStyle, GUILayout.Height(20f)))
                    {
                        tagsProperty.arraySize++;
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(30f), GUILayout.MinHeight(100f));
                    if (GUILayout.Button("-", buttonRemoveStyle))
                    {
                        Map.PresetRemove(i);
                        SceneDrawer.Redraw();
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        break;
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }

                if (GUILayout.Button("+", buttonAddStyle, GUILayout.Height(20)))
                {
                    Map.PresetAddDefault();
                    if (IsSceneEditing)
                    {
                        SceneDrawer.EditingUpdate();
                    }
                }
                serializedMap.ApplyModifiedProperties();

                TilePresetCurrent = Mathf.Clamp(TilePresetCurrent, 0, Map.Presets.Count - 1);
                EditorGUIUtility.labelWidth = labelWidth;

                TileIds = Map.Presets.Select(t => t.Id).ToArray();
                SceneDrawer.TileIds = TileIds;
                SceneDrawer.TileType = TilePresetCurrent;

                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
            if (SceneDrawer != null)
            {
                SceneDrawer.OnBeforeChanged += UndoRecord;
            }
        }

        void OnLostFocus()
        {
            if (SceneDrawer != null)
            {
                SceneDrawer.OnBeforeChanged -= UndoRecord;
            }
        }

        void OnChangedMap()
        {
            if (Map)
            {
                GridType = (int)Map.Type;
                GridAxisType = (int)Map.Axis;
                RotationType = (int)Map.RotationType;
            }
            SceneDrawer.Map = Map;
            SceneDrawer.Redraw();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            SceneDrawer.Draw(IsSceneEditing, ShowMap);
            SceneView.RepaintAll();
        }

        void InitResources()
        {
            Settings = MapWindowSettings.Instance;
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }

        void UndoRecord()
        {
            Undo.RegisterCompleteObjectUndo(this, "Map");
        }

        void DrawToolTiles()
        {
            var toolStyleNormal = Skin.customStyles[7];
            var toolStyleSelected = Skin.customStyles[8];
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Tile", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            if (ButtonTwoStyle(Settings.BrushIcon, TileToolNames[0], toolStyleNormal, toolStyleSelected, ToolToogle[0]))
            {
                ToolToogle[0] = !ToolToogle[0];
            }

            if (ButtonTwoStyle(Settings.EraseIcon, TileToolNames[1], toolStyleNormal, toolStyleSelected, ToolToogle[1]))
            {
                ToolToogle[1] = !ToolToogle[1];
            }
            EditorGUILayout.EndHorizontal();
            GuiStyles.DrawHorizontal(Separator);
            EditorGUILayout.EndVertical();
        }

        void DrawToolEdges()
        {
            var toolStyleNormal = Skin.customStyles[7];
            var toolStyleSelected = Skin.customStyles[8];
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Edge", EditorStyles.boldLabel);
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = ToolLabelWidth;
            EditorGUILayout.BeginHorizontal();
            if (ButtonTwoStyle(Settings.BrushIcon, EdgeToolNames[0], toolStyleNormal, toolStyleSelected, ToolToogle[2]))
            {
                ToolToogle[2] = !ToolToogle[2];
            }
            if (ButtonTwoStyle(Settings.EraseIcon, EdgeToolNames[1], toolStyleNormal, toolStyleSelected, ToolToogle[3]))
            {
                ToolToogle[3] = !ToolToogle[3];
            }
            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = labelWidth;
            GuiStyles.DrawHorizontal(Separator);
            EditorGUILayout.EndVertical();
        }

        void SceneEditingStart()
        {
            SceneDrawer.Map = Map;
            SceneDrawer.EditingUpdate();
        }

        void SceneEditingFinish()
        {
            SceneDrawer.EditingFinish();
            MarkAreas();
        }

        void MapCreateAsset()
        {
            var scene = EditorSceneManager.GetActiveScene();
            var filename = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            var directory = System.IO.Path.GetDirectoryName(scene.path);
            var mapPath = MapSettings.Path(directory, filename, Grids[GridType]);
            mapPath = AssetDatabase.GenerateUniqueAssetPath(mapPath);
            var mapInstance = ScriptableObject.CreateInstance<MapSettings>();
            mapInstance.Init((GridType)GridType, (GridAxis)GridAxisType, Settings.Rules, Settings.CellBorder);
            AssetDatabase.CreateAsset(mapInstance, mapPath);
            AssetDatabase.SaveAssets();
            Map = mapInstance;
        }

        bool ToolValidate()
        {
            return Map.Presets != null && Map.Presets.Count > 0;
        }

        bool ButtonTwoStyle(Texture2D icon, string tooltip, GUIStyle normal, GUIStyle selected, bool state)
        {
            var toolPressed = false;
            if (state)
            {
                toolPressed = GUILayout.Button(new GUIContent(icon, tooltip), selected, GUILayout.Width(32f), GUILayout.Height(32f));
            }
            else
            {
                toolPressed = GUILayout.Button(new GUIContent(icon, tooltip), normal, GUILayout.Width(32f), GUILayout.Height(32f));
            }
            return toolPressed;
        }

        bool ButtonTwoStyle(GUIStyle normal, GUIStyle selected, bool state)
        {
            var toolPressed = false;
            if (state)
            {
                toolPressed = GUILayout.Button("+", selected, GUILayout.Width(24f));
            }
            else
            {
                toolPressed = GUILayout.Button(" ", normal, GUILayout.Width(24f));
            }
            return toolPressed;
        }

        /// <summary>
        /// Place tile presets prefabs to scene
        /// </summary>
        public void PlacePrefabs()
        {
            switch (Map.Type)
            {
                case RedBjorn.ProtoTiles.GridType.HexFlat: PlacePrefabsHexFlat(); break;
                case RedBjorn.ProtoTiles.GridType.HexPointy: PlacePrefabsHexPointy(); break; 
                case RedBjorn.ProtoTiles.GridType.Square: PlacePrefabsSquare(); break;
            }
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
#endif
        }

        void PlacePrefabsHexFlat()
        {
            var holder = GetMapHolder(true);
            var parent = new GameObject("Tiles");
            parent.transform.SetParent(holder.transform);
            parent.transform.localPosition = Vector3.zero;

            foreach (var t in Map.Tiles)
            {
                var preset = Map.Presets.FirstOrDefault(p => p.Id == t.Id);
                if (preset != null)
                {
                    var prefab = Map.Presets.FirstOrDefault(p => p.Id == t.Id).Prefab;
                    if (prefab != null)
                    {
#if UNITY_EDITOR
                        var tileGo = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
                        tileGo.transform.localRotation = Quaternion.Inverse(parent.transform.rotation);
                        tileGo.transform.position = HexFlat.ToWorld(t.TilePos, Map.Axis, Map.Edge);
                        Undo.RegisterCreatedObjectUndo(tileGo, "Create MapView");
#endif
                    }
                }
            }
        }

        void PlacePrefabsHexPointy()
        {
            var holder = GetMapHolder(true);
            var parent = new GameObject("Tiles");
            parent.transform.SetParent(holder.transform);
            parent.transform.localPosition = Vector3.zero;

            foreach (var t in Map.Tiles)
            {
                var preset = Map.Presets.FirstOrDefault(p => p.Id == t.Id);
                if (preset != null)
                {
                    var prefab = Map.Presets.FirstOrDefault(p => p.Id == t.Id).Prefab;
                    if (prefab != null)
                    {
#if UNITY_EDITOR
                        var tileGo = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
                        tileGo.transform.localRotation = Quaternion.Inverse(parent.transform.rotation);
                        tileGo.transform.position = HexPointy.ToWorld(t.TilePos, Map.Axis, Map.Edge);
                        Undo.RegisterCreatedObjectUndo(tileGo, "Create MapView");
#endif
                    }
                }
            }
        }

        void PlacePrefabsSquare()
        {
            var holder = GetMapHolder(true);
            var parent = new GameObject("Tiles");
            parent.transform.SetParent(holder.transform);
            parent.transform.localPosition = Vector3.zero;
            foreach (var t in Map.Tiles)
            {
                var preset = Map.Presets.FirstOrDefault(p => p.Id == t.Id);
                if (preset != null)
                {
                    var prefab = Map.Presets.FirstOrDefault(p => p.Id == t.Id).Prefab;
                    if (prefab != null)
                    {
#if UNITY_EDITOR
                        var tileGo = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
                        tileGo.transform.localRotation = Quaternion.Inverse(parent.transform.rotation);
                        tileGo.transform.position = Square.ToWorld(t.TilePos, Map.Axis, Map.Edge);
                        Undo.RegisterCreatedObjectUndo(tileGo, "Create MapView");
#endif
                    }
                }
            }
        }

        MapView GetMapHolder(bool doClean = false)
        {
#if UNITY_EDITOR
            var holder = GameObject.FindObjectOfType<MapView>();
            if (holder == null)
            {
                var holderGo = new GameObject();
                holderGo.name = "MapView";
                holderGo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                holder = holderGo.AddComponent<MapView>();
                Undo.RegisterCreatedObjectUndo(holderGo, "Create Map View");
            }

            if (doClean)
            {
                for (int i = holder.transform.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(holder.transform.GetChild(i).gameObject);
                }
            }
            return holder;
#else
            return null;
#endif
        }

        public static MapView CreateHolder()
        {
            var holderGo = new GameObject();
            holderGo.name = "MapView";
            holderGo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            var holder = holderGo.AddComponent<MapView>();
            return holder;
        }

        /// <summary>
        /// Map View at Scene View is empty?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                var holder = GetMapHolder();
                return holder == null || holder.transform.childCount == 0;
            }
        }

        /// <summary>
        /// Calculate tile area ownership
        /// </summary>
        public void MarkAreas()
        {
            switch (Map.Type)
            {
                case RedBjorn.ProtoTiles.GridType.HexFlat: MarkAreas(Hex.Distance, Hex.Neighbour); break;
                case RedBjorn.ProtoTiles.GridType.HexPointy: MarkAreas(Hex.Distance, Hex.Neighbour); break;
                case RedBjorn.ProtoTiles.GridType.Square: MarkAreas(Square.Distance, Square.Neighbour); break;
            }
        }

        void MarkAreas(Func<Vector3Int, Vector3Int, float> DistanceFunc, Vector3Int[] NeighboursDirection)
        {
            var map = new MapEntityMock();
            map.DistanceFunc = DistanceFunc;
            map.NeighboursDirection = NeighboursDirection;
            for (int i = 0; i < Map.Tiles.Count; i++)
            {
                var tilePreset = Map.Tiles[i];
                var type = Map.Presets.FirstOrDefault(t => t.Id == tilePreset.Id);
                map.Tiles[Map.Tiles[i].TilePos] = new TileEntity(tilePreset, type, Map.Rules);
            }
            int movableArea = 1;
            var marked = new HashSet<INode>();
            var walkableCount = map.Tiles.Count(t => t.Value.Vacant);
            while (marked.Count < walkableCount)
            {
                var walkable = map.Tiles.FirstOrDefault(t => t.Value.Vacant && !marked.Any(m => m.Position == t.Value.Position));
                if (walkable.Value != null)
                {
                    var accessible = NodePathFinder.AccessibleArea(map, walkable.Value);
                    foreach (var a in accessible)
                    {
                        a.ChangeMovableAreaPreset(movableArea);
                        marked.Add(a);
                    }
                }
                else
                {
                    break;
                }
                movableArea++;
            }

            foreach (var t in map.Tiles.Where(t => !t.Value.Vacant))
            {
                t.Value.Data.MovableArea = 0;
            }
        }
    }
}
