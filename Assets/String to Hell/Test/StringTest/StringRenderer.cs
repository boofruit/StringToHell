using System.Collections.Generic;
using UnityEngine;

namespace StringToHell.Test.StringTest
{
    public class StringRenderer : MonoBehaviour
    {
        public LineRenderer line;
        public List<Transform> segments;
        public float textureTilingMultiplier = 1f;

        void Update()
        {
            line.positionCount = segments.Count;

            float totalLength = 0f;

            for (int i = 0; i < segments.Count; i++)
            {
                line.SetPosition(i, segments[i].position);

                if (i > 0)
                    totalLength += Vector2.Distance(segments[i].position, segments[i - 1].position);
            }

            // Tile texture based on rope length
            line.material.mainTextureScale = new Vector2(totalLength * textureTilingMultiplier, 1);
        }
    }
}
