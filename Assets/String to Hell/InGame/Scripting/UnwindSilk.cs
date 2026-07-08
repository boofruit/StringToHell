using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace StringToHell.InGame
{
    public class UnwindSilk : MonoBehaviour, IUnwindSilk
    {
        Transform tf;
        Rigidbody2D anchor;
        // Where rope starts
        GameObject spawner;
        Vector2 spawnPoint;             // Where rope starts
        [SerializeField, Tooltip("")] GameObject segmentPrefab;         // Rope segment prefab
        [SerializeField, Tooltip("")] LineRenderer line;                // Visual rope

        private List<Transform> segments = new List<Transform>();
        SpringJoint2D BaseJoint;

        StringManager stringManager;
        bool isUnwinding = false;
        bool lineConnnected = false;
        public bool LineConnected => lineConnnected;
        Vector2 lastSpawnPoint;
        float segmentSpacing;
        bool tugging = false;
        public bool Tugging => tugging;
        Vector2 slingDirection;
        public Vector2 SlingDirection => slingDirection;
        float bungieForce;

        void Start()
        {
            tf = transform;
            spawner = this.gameObject;
            BaseJoint = GetComponentInParent<SpringJoint2D>();
        }
        public void Extinguish()
        {
            //segments.Remove(spawner.transform);
            stringManager.Segments.Remove(spawner.transform);
            
            segments.Clear();
            tugging = false;
            isUnwinding = false;
            lineConnnected = false; 
           // Debug.Log("Extinguished");
        }

        public void StartThread(Rigidbody2D newAnchor, float spacing)
        {
            if(lineConnnected) { return; }
            segmentSpacing = spacing;
            lineConnnected = true;
            anchor = newAnchor;
            stringManager = anchor.GetComponent<StringManager>();
            stringManager.spacing = spacing;
            BaseJoint.enabled = false;
            BaseJoint.connectedBody = anchor;

            spawnPoint = tf.position;
            lastSpawnPoint = spawnPoint;
            segments.Add(anchor.transform);
            stringManager.Segments.Add(anchor.transform);
            isUnwinding = true;
        }
        public void StopThread()
        {
            if(segments.Count == 0|| !lineConnnected || !isUnwinding) return;
           // segments.Add(spawner.transform);
            stringManager.Segments.Add(spawner.transform);
            BaseJoint.enabled = true;
            isUnwinding = false;
           // Debug.Log("Holding Thread");
        }


        public void AddSegment( int maxSegementsLength, float frequency, float dampingRatio, float spacingMultiplier)
        {
            if (!isUnwinding || segments.Count >= maxSegementsLength || segments.Count == 0) return;
            spawnPoint = tf.position;
            if ((spawnPoint - lastSpawnPoint).magnitude < segmentSpacing * spacingMultiplier) return;
           
            Transform last = segments.LastOrDefault();

            Vector2 newPos = last.position - last.up * segmentSpacing;

            GameObject seg = Instantiate(segmentPrefab, spawnPoint, Quaternion.identity);
            segments.Add(seg.transform);
            stringManager.Segments.Add(seg.transform);
            seg.transform.SetParent(anchor.transform, true);
            //seg.transform.localPosition = newPos;

            SpringJoint2D dist = seg.GetComponent<SpringJoint2D>();

            dist.connectedBody = last.GetComponent<Rigidbody2D>();
            BaseJoint.connectedBody = segments.LastOrDefault()?.GetComponent<Rigidbody2D>();
            BaseJoint.enabled = true;
            //// Distance joint for elasticity
            dist.autoConfigureDistance = false;
            dist.distance = segmentSpacing;
            dist.frequency = frequency;
            dist.dampingRatio = dampingRatio;
            lastSpawnPoint = spawnPoint;

        }

        //public void BungieSling(float slingForce)
        //{
        //    var PlayerRB = BaseJoint.GetComponent<Rigidbody2D>();
        //    Vector2 slingDirection = (Vector2)(BaseJoint.anchor - BaseJoint.connectedBody.position  );
        //    slingForce *= slingDirection.magnitude - spacing;
        //    PlayerRB.AddForce(slingDirection * slingForce, ForceMode2D.Impulse);
        //}
        public void CalculateStrech(float slingForce, float minTension, float maxPower)
        {
            if(!lineConnnected || isUnwinding) return;
        
        int SegementsPower = segments.Count;
            // Convert anchor to world space
            Vector2 worldAnchor = BaseJoint.transform.TransformPoint(BaseJoint.anchor);

            // Direction from player to anchor
            Vector2 direction = worldAnchor - anchor.position;

            float distance = direction.magnitude;

            // Normalize direction so it only represents direction, not magnitude
            slingDirection = direction.normalized * -1;

            // How far past the rest length (spacing) the spring is stretched
            float stretch = (distance / SegementsPower) - segmentSpacing;
            // Scale force by stretch amount
            float finalForce = Mathf.Clamp(slingForce * stretch, minTension, maxPower);
            if (finalForce > minTension) { 
                tugging = true; 
                bungieForce = finalForce;
            }
            else { tugging = false; }
            
        }
        public void BungieSling()
        {
            if (segments.Count == 0 || anchor == null) return;
            if (isUnwinding || !lineConnnected) return;
            Rigidbody2D rb = BaseJoint.GetComponent<Rigidbody2D>();

            if (!tugging)
                return; // No tension, no force
            rb.linearDamping = 1;
            //rb.linearVelocity *= 0f;
            rb.AddForce(slingDirection * bungieForce, ForceMode2D.Impulse);
            bungieForce = 0;
            slingDirection = Vector2.zero;
            tugging = false;
        }
        public void CutThread()
        {
            if (!lineConnnected || segments.Count == 0) return;
            BaseJoint.connectedBody = null;
            BaseJoint.enabled = false;
            Extinguish();
        }
        public void ConnectLine(GameObject WebJoint)
        {
            if (!lineConnnected || segments.Count == 0 || isUnwinding) return;
            
            var lastSegment = segments.LastOrDefault()?.GetComponent<Rigidbody2D>();
            if (lastSegment != null)
            {
                WebJoint.TryGetComponent<HingeJoint2D>(out var hingeJoint);
                hingeJoint.enabled = true;
                hingeJoint.connectedBody = lastSegment;
                lastSegment.transform.position = WebJoint.transform.position;
            }
            Extinguish();
        }

        public void UpdateLineRenderer()
        {
            if (segments.Count == 0) return;
            line.positionCount = segments.Count;
            int index = 0;
            foreach (var seg in segments)
            {
                line.SetPosition(index++, seg.position);
            }
            // Optional: tile texture based on rope length
            float totalLength = (line.positionCount) * segmentSpacing;
            line.material.mainTextureScale = new Vector2(totalLength, 1);
        }
    }
}

