using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles.Example
{
    public class LineDrawer : MonoBehaviour
    {
        public LineRenderer Line;

        public void Show(Vector3 start, Vector3 end)
        {
            Line.positionCount = 2;
            Line.SetPosition(0, start);
            Line.SetPosition(1, end);
            Line.enabled = true;
        }

        public void Show(List<Vector3> points)
        {
            Show(points.ToArray());
        }

        public void Show(Vector3[] points)
        {
            Line.positionCount = points.Length;
            Line.SetPositions(points);
            Line.enabled = true;
        }

        public void Show(Vector3 start, Vector3 end, Color color)
        {
            SetColor(color);
            Show(start, end);
        }

        public void Show(List<Vector3> points, Color color)
        {
            SetColor(color);
            Show(points.ToArray());
        }

        public void Show(Vector3[] points, Color color)
        {
            SetColor(color);
            Show(points);
        }

        void SetColor(Color color)
        {
            Line.startColor = color;
            Line.endColor = color;
        }

        public void Hide()
        {
            Line.enabled = false;
        }
    }
}