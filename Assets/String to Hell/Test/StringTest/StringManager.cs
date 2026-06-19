using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

namespace StringToHell.InGame
{
    public class StringManager : MonoBehaviour
    {
        public LineRenderer line;
        public EdgeCollider2D edgeCollider;
        public EdgeCollider2D triggerCollider;
        public List<Transform> Segments;
        public float textureTilingMultiplier = 1f;
        public float spacing;

        void Update()
        {
            UpdateLineRenderer();
        }
        void UpdateLineRenderer()
        {
            if (Segments == null) return;
            line.positionCount = Segments.Count;
            int index = 0;
            foreach (var seg in Segments)
            {
                line.SetPosition(index++, seg.position);
            }

            List<Vector2> colliderPoints = new List<Vector2>(Segments.Count);
          
            foreach (var seg in Segments)
            {
                colliderPoints.Add(seg.position - transform.position);
            }
            edgeCollider.SetPoints(colliderPoints);
            triggerCollider.SetPoints(colliderPoints);
            // Optional: tile texture based on rope length
            float totalLength = (line.positionCount) * spacing;
            line.material.mainTextureScale = new Vector2(totalLength * textureTilingMultiplier, 1);
        }
    }
}
