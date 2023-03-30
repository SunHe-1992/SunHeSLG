using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.ProtoTiles.Example
{
    public class PathDrawer : MonoBehaviour
    {
        public LineDrawer Line;
        public SpriteRenderer Tail;
        public Color ActiveColor;
        public Color InactiveColor;

        public bool IsEnabled { get; set; }

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
            Tail.color = color;
        }

        public void Show(List<Vector3> points, MapEntity map)
        {
            var offset = map.Settings.VectorCreateOrthogonal(0.01f);
            Line.Line.transform.localPosition = offset;
            Tail.transform.localPosition = offset;
            Tail.transform.rotation = map.Settings.RotationPlane();

            if (points == null || points.Count == 0)
            {
                Hide();
            }
            else
            {
                var tailPos = points[points.Count - 1];
                Tail.transform.localPosition = map.Settings.Projection(tailPos, 0.01f);
                Tail.gameObject.SetActive(true);
                if (points.Count > 1)
                {
                    Line.Line.transform.localRotation = map.Settings.RotationPlane();
                    points[points.Count - 1] = (points[points.Count - 1] + points[points.Count - 2]) / 2f;
                    var pointsXY = new Vector3[points.Count];
                    for (int i = 0; i < pointsXY.Length; i++)
                    {
                        pointsXY[i] = map.Settings.ProjectionXY(points[i]);
                    }

                    Line.Show(pointsXY);
                }
            }
        }

        public void Hide()
        {
            Line.Hide();
            Tail.gameObject.SetActive(false);
        }
    }
}