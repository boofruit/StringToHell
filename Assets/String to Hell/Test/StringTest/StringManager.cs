using System.Collections.Generic;
using UnityEngine;

namespace StringToHell.InGame
{
    public class StringManager : MonoBehaviour
    {
        public LineRenderer line;
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
            // Optional: tile texture based on rope length
            float totalLength = (line.positionCount) * spacing;
            line.material.mainTextureScale = new Vector2(totalLength * textureTilingMultiplier, 1);
        }
    }
}
