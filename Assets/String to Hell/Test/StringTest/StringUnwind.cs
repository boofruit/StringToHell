using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace StringToHell.Test.StringTest
{
    public class StringUnwind : MonoBehaviour
    {

        Rigidbody2D anchor;
        // Where rope starts
        GameObject spawner;
        Vector2 spawnpoint;             // Where rope starts
        [SerializeField] GameObject segmentPrefab;         // Rope segment prefab
        [SerializeField] LineRenderer line;                // Visual rope
        [SerializeField] float segmentSpacing = 0.25f;     // Distance between segments
        [SerializeField] float unwindSpeed = 2f;           // Segments per second
        [SerializeField] float frequency = 5f;              // Elasticity strength
        [SerializeField] float dampingRatio = 0.4f;        // Reduces wobble
        private List<Transform> segments = new List<Transform>();
        SpringJoint2D BaseJoint;
        [SerializeField] int maxSegementsLength = 20;
        private float unwindTimer = 0f;
        bool isUnwinding = false;
        bool lineExtinguished = false;
        public bool LineExtinguished => lineExtinguished;
        Vector2 lastSpawnPoint;
       


        void Start()
        {
          anchor = GetComponent<Rigidbody2D>();
        }
        public  void StartThread(Rigidbody2D newAnchor, GameObject newSpawner)
        {
            anchor = newAnchor;
            spawner = newSpawner;
            BaseJoint = spawner.GetComponent<SpringJoint2D>();
            BaseJoint.enabled = true;
            BaseJoint.connectedBody = anchor;
            
            spawnpoint = spawner.transform.position;
            lastSpawnPoint = spawnpoint;
            segments.Add(anchor.transform);
            isUnwinding = true;
        }
        public void StopThread()
        {
            segments.Add(BaseJoint.transform);
            isUnwinding = false;
        }
        void Update()
        {

            UpdateLineRenderer();
        }

        public void AddSegment(Vector2 spawnPoint)
        {
            if (!isUnwinding || segments.Count >= maxSegementsLength) return;
            if ((spawnPoint - lastSpawnPoint).magnitude < segmentSpacing * 1.5f) return;

            Transform last = segments.LastOrDefault();

            Vector2 newPos = last.position - last.up * segmentSpacing;

            GameObject seg = Instantiate(segmentPrefab, spawnpoint, Quaternion.identity);
            segments.Add(seg.transform);

            SpringJoint2D dist = seg.GetComponent<SpringJoint2D>();
            
             dist.connectedBody = last.GetComponent<Rigidbody2D>();
            BaseJoint.connectedBody = segments.LastOrDefault()?.GetComponent<Rigidbody2D>();
            //// Distance joint for elasticity
            dist.autoConfigureDistance = false;
            dist.distance = segmentSpacing;
            dist.frequency = frequency;     
            dist.dampingRatio = dampingRatio; 
            lastSpawnPoint = spawnPoint;
          
        }
        public void CutThread()
        {
            segments.Remove(BaseJoint.transform);
            BaseJoint.connectedBody = null;
            BaseJoint.enabled = false;
            lineExtinguished = true;
        }
        public void ConnectLine(GameObject WebJoint)
        {
            if(lineExtinguished) return;
            segments.Remove(BaseJoint.transform);
            var lastSegment = segments.LastOrDefault()?.GetComponent<Rigidbody2D>();
            if (lastSegment != null)
            {
                WebJoint.TryGetComponent<HingeJoint2D>(out var hingeJoint);
                hingeJoint.enabled = true;
                hingeJoint.connectedBody = lastSegment;
                lastSegment.transform.position = WebJoint.transform.position;
            }
            lineExtinguished = true;
            
        }

        void UpdateLineRenderer()
        {
            line.positionCount = segments.Count ;
            int index = 0;
            foreach (var seg in segments)
            {
                line.SetPosition(index++, seg.position);
            }
            // Optional: tile texture based on rope length
            float totalLength = (line.positionCount ) * segmentSpacing;
            line.material.mainTextureScale = new Vector2(totalLength, 1);
        }
    }
}

