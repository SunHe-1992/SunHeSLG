using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles.Example
{
    public class AreaOutline : MonoBehaviour
    {
        public LineDrawer Line;
        public Color ActiveColor;
        public Color InactiveColor;

        const float Offset = 0.01f;

        public void ActiveState()
        {
            SetColor(ActiveColor);
        }

        public void InactiveState()
        {
            SetColor(InactiveColor);
        }

        void SetColor(Color color)
        {
            Line.Line.material.color = color;
        }

        public void Show(List<Vector3> points, MapEntity map)
        {
            Line.Line.transform.localPosition = map.Settings.VectorCreateOrthogonal(Offset);
            Line.Line.transform.localRotation = map.Settings.RotationPlane();

            var pointsXY = new Vector3[points.Count];
            for (int i = 0; i < pointsXY.Length; i++)
            {
                pointsXY[i] = map.Settings.ProjectionXY(points[i]);
            }
            Line.Show(pointsXY);
        }

        public void Hide()
        {
            Line.Hide();
        }
    }
}
