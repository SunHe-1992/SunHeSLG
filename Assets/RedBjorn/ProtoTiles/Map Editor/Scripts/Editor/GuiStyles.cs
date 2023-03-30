using UnityEditor;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    public class GuiStyles
    {
        public readonly static GUIStyle HorizontalLine = new GUIStyle
        {
            normal = new GUIStyleState() { background = EditorGUIUtility.whiteTexture },
            fixedHeight = 4f,
            margin = new RectOffset(0, 0, 8, 8)
        };

        public readonly static GUIStyle CenterAligment = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter
        };

        public static void DrawHorizontal(Color color)
        {
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, HorizontalLine);
            GUI.color = c;
        }

        public static void DrawHexFlat(Vector3 center, Color color, float edge, GridAxis axis)
        {
            var vertices = HexFlat.Vertices(axis);
            var v1 = edge * vertices[0] + center;
            var v2 = edge * vertices[1] + center;
            var v3 = edge * vertices[2] + center;
            var v4 = edge * vertices[3] + center;
            var v5 = edge * vertices[4] + center;
            var v6 = edge * vertices[5] + center;

            var c = Handles.color;
            Handles.color = color;
            Handles.DrawAAConvexPolygon(v1, v2, v3, v4, v5, v6);
            Handles.color = c;
        }

        public static void DrawHexPointy(Vector3 center, Color color, float edge, GridAxis axis)
        {
            var vertices = HexPointy.Vertices(axis);
            var v1 = edge * vertices[0] + center;
            var v2 = edge * vertices[1] + center;
            var v3 = edge * vertices[2] + center;
            var v4 = edge * vertices[3] + center;
            var v5 = edge * vertices[4] + center;
            var v6 = edge * vertices[5] + center;

            var c = Handles.color;
            Handles.color = color;
            Handles.DrawAAConvexPolygon(v1, v2, v3, v4, v5, v6);
            Handles.color = c;
        }

        public static void DrawSquare(Vector3 center, Color color, float edge, GridAxis axis)
        {
            var vertices = Square.Vertices(axis);
            var v1 = edge * vertices[0] + center;
            var v2 = edge * vertices[1] + center;
            var v3 = edge * vertices[2] + center;
            var v4 = edge * vertices[3] + center;
            
            var c = Handles.color;
            Handles.color = color;
            Handles.DrawAAConvexPolygon(v1, v2, v3, v4);
            Handles.color = c;
        }

        public static void DrawX(Vector3 center, Color color, float edge)
        {
            var c = Handles.color;
            Handles.color = color;
            var half = edge / 2f;
            var v1 = edge * new Vector3(-half, 0f, half) + center;
            var v2 = edge * new Vector3(half, 0f, -half) + center;
            var v3 = edge * new Vector3(half, 0f, half) + center;
            var v4 = edge * new Vector3(-half, 0f, -half) + center;
            Handles.DrawLine(v1, v2);
            Handles.DrawLine(v3, v4);
            Handles.color = c;
        }

        public static void DrawCircle(Vector3 center, Color color, float radius)
        {
            var c = Handles.color;
            Handles.color = color;
            Handles.DrawSolidArc(center, Vector3.up, Vector3.forward, 360f, radius);
            Handles.color = c;
        }

        public static void DrawRect(Vector3 center, Color color, float angle, float width, float height, GridAxis axis)
        {
            var c = Handles.color;
            Handles.color = color;
            var v1 = Vector3.zero;
            var v2 = Vector3.zero;
            var v3 = Vector3.zero;
            var v4 = Vector3.zero;
            if (axis == GridAxis.XZ)
            {
                v1 = Quaternion.Euler(0f, angle, 0f) * new Vector3(-width / 2f, 0f,  height / 2f) + center;
                v2 = Quaternion.Euler(0f, angle, 0f) * new Vector3( width / 2f, 0f,  height / 2f) + center;
                v3 = Quaternion.Euler(0f, angle, 0f) * new Vector3( width / 2f, 0f, -height / 2f) + center;
                v4 = Quaternion.Euler(0f, angle, 0f) * new Vector3(-width / 2f, 0f, -height / 2f) + center;
            }
            else if (axis == GridAxis.XY)
            {
                v1 = Quaternion.Euler(0f, 0f, angle) * new Vector3(-width / 2f,  height / 2f, 0f) + center;
                v2 = Quaternion.Euler(0f, 0f, angle) * new Vector3( width / 2f,  height / 2f, 0f) + center;
                v3 = Quaternion.Euler(0f, 0f, angle) * new Vector3( width / 2f, -height / 2f, 0f) + center;
                v4 = Quaternion.Euler(0f, 0f, angle) * new Vector3(-width / 2f, -height / 2f, 0f) + center;
            }
            else if (axis == GridAxis.ZY)
            {
                v1 = Quaternion.Euler(angle, 0f, 0f) * new Vector3(0f,  height / 2f, -width / 2f) + center;
                v2 = Quaternion.Euler(angle, 0f, 0f) * new Vector3(0f,  height / 2f,  width / 2f) + center;
                v3 = Quaternion.Euler(angle, 0f, 0f) * new Vector3(0f, -height / 2f,  width / 2f) + center;
                v4 = Quaternion.Euler(angle, 0f, 0f) * new Vector3(0f, -height / 2f, -width / 2f) + center;
            }

            Handles.DrawAAConvexPolygon(v1, v2, v3, v4);
            Handles.color = c;
        }

        public static void DrawLabel(string text, Vector3 center, Color color)
        {
            var c = Handles.color;
            Handles.color = color;
            Handles.Label(center, text);
            Handles.color = c;
        }
    }
}