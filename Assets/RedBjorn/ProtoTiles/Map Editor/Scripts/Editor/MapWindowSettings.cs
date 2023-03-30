using RedBjorn.Utils;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    [CreateAssetMenu(menuName = "RedBjorn/ProtoTiles/Map Window Settings")]
    public class MapWindowSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class Theme
        {
            public Color CommonColor;
            public Color SeparatorColor;
            public Color WorkAreaColor;
        }

        public Theme Light;
        public Theme Dark;

        public MapRules Rules;

        public float CommonHeight = 140;
        public float Border = 4;
        public float ToolLabelWidth = 40f;
        public float TileLabelWidth = 80f;


        public Color LabelColor = Color.black;
        public Color EdgeColor = Color.red;
        public Color EdgeCursorColor = Color.yellow;
        public Color EdgeCursorPaint = Color.green;
        public Color EdgeCursorErase = Color.red;
        public Color TileCursorErase = new Color(1f, 1f, 1f, 0.7f);

        public Texture2D BrushIcon;
        public Texture2D EraseIcon;
        public GUISkin Skin;

        [HideInInspector] public bool DrawSideTool;
        public bool AreasAutoMark;
        public Material CellBorder;

        public const string DefaultPathFull = Utils.Paths.ScriptablePath.RootFolder + "/" + DefaultPathRelative;
        public const string DefaultPathRelative = "RedBjorn/ProtoTiles/Map Editor/Editor Resources/MapWindowSettings.asset";
        public static Vector2 WindowMinSize = new Vector2(270f, 480f);

        public Color CommonColor => EditorGUIUtility.isProSkin ? Dark.CommonColor : Light.CommonColor;
        public Color WorkAreaColor => EditorGUIUtility.isProSkin ? Dark.WorkAreaColor : Light.WorkAreaColor;
        public Color Separator => EditorGUIUtility.isProSkin ? Dark.SeparatorColor : Light.SeparatorColor;

        public static MapWindowSettings Instance
        {
            get
            {
                var path = DefaultPathFull;
                var instance = AssetDatabase.LoadAssetAtPath<MapWindowSettings>(DefaultPathFull);
                if (!instance)
                {
                    var paths = AssetDatabase.FindAssets("t:" + nameof(MapWindowSettings))
                         .Select(a => AssetDatabase.GUIDToAssetPath(a))
                         .OrderBy(a => a);
                    path = paths.FirstOrDefault(i => i.Contains(DefaultPathRelative));
                    instance = AssetDatabase.LoadAssetAtPath<MapWindowSettings>(path);
                    if (!instance)
                    {
                        path = paths.FirstOrDefault();
                        instance = AssetDatabase.LoadAssetAtPath<MapWindowSettings>(path);
                    }
                }
                return instance;
            }
        }
    }
}
