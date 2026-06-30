using System.Collections.Generic;
using System.Linq;
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
        public float windForce;
        public float maxWindForce;

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
        private void OnTriggerStay2D(Collider2D collision)
        {
            var entering = collision.gameObject;
            if (entering.CompareTag("Wind"))
            {

                var wind = entering.GetComponent<AreaEffector2D>();
                float angle = wind.forceAngle;
                float rad = angle * Mathf.Deg2Rad;

                var forceDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                float force = Mathf.Clamp( wind.forceMagnitude,0f, maxWindForce);
               foreach( var seg in Segments)
                {
                    if (seg.CompareTag("Player")|| seg == Segments.FirstOrDefault())
                    {
                        continue;
                    }
                    Vector2 effectorVelocity = forceDirection * force * Time.fixedDeltaTime;
                    seg.GetComponent<Rigidbody2D>().linearVelocity += effectorVelocity;
                }
                  
                
            }
        }
    }
}
