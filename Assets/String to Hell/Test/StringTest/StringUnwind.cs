using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace StringToHell.Test.StringTest
{
    public class StringUnwind : MonoBehaviour
    {


        public Rigidbody2D anchor;                 // Where rope starts
        public Vector2 spawnpoint;             // Where rope starts
        public GameObject segmentPrefab;         // Rope segment prefab
        public LineRenderer line;                // Visual rope
        public float segmentSpacing = 0.25f;     // Distance between segments
        public float unwindSpeed = 2f;           // Segments per second
        public float frequency = 5f;              // Elasticity strength
        public float dampingRatio = 0.4f;        // Reduces wobble
        private List<Transform> segments = new List<Transform>();
        SpringJoint2D lastSegment;
        [SerializeField] int maxSegementsLength = 20;
        private float unwindTimer = 0f;
        bool isUnwinding = false;
        Vector2 lastSpawnPoint;

        void Start()
        {
          anchor = GetComponent<Rigidbody2D>();
        }
        public  void StartThread(Rigidbody2D anchor, GameObject spawnObject)
        {
            this.anchor = anchor;
            spawnpoint = spawnObject.transform.position;
            lastSpawnPoint = spawnpoint;
            // Create first segment at anchor
            //GameObject first = Instantiate(segmentPrefab, anchor.position, Quaternion.identity);
            segments.Add(anchor.transform);

            //// Connect first segment to anchor
            //first.GetComponent<SpringJoint2D>().connectedBody = anchor.GetComponent<Rigidbody2D>();
            //lastSegment = first.GetComponent<SpringJoint2D>();
            //lastSegment.distance = segmentSpacing;
            //lastSegment.frequency = frequency;
            //lastSegment.dampingRatio = dampingRatio;
            isUnwinding = true;
        }
        public void StopThread()
        {
            isUnwinding = false;
            var joint = this.GetComponent<SpringJoint2D>();
                joint.connectedBody = segments[-1].GetComponent<Rigidbody2D>(); 
            joint.enabled = true;
           
            //lastSegment.connectedBody = spawnpoint.GetComponent<Rigidbody2D>();
            //lastSegment.transform.position = spawnpoint.transform.position;
        }
        void Update()
        {
            //unwindTimer += Time.deltaTime;

            //// Add new segments over time
            //if (unwindTimer >= 1f / unwindSpeed)
            //{
            //    unwindTimer = 0f;
            //    AddSegment();
            //}

            UpdateLineRenderer();
        }

        public void AddSegment(Vector2 spawnPoint)
        {
            if (!isUnwinding || segments.Count >= maxSegementsLength) return;
            if ((spawnPoint - lastSpawnPoint).magnitude < segmentSpacing) return;

            Transform last = segments[segments.Count - 1];

            Vector2 newPos = last.position - last.up * segmentSpacing;

            GameObject seg = Instantiate(segmentPrefab, spawnpoint, Quaternion.identity);
            segments.Add(seg.transform);

            SpringJoint2D dist = seg.GetComponent<SpringJoint2D>();
            
             dist.connectedBody = last.GetComponent<Rigidbody2D>(); 
               
            lastSegment = dist;


            //if (unwindTimer >= 1f / unwindSpeed)
            //{
            //    unwindTimer = 0f;
            //}

            //// Hinge joint for bending
            //HingeJoint2D hinge = seg.GetComponent<HingeJoint2D>();
            //hinge.connectedBody = last.GetComponent<Rigidbody2D>();

            //// Distance joint for elasticity
            dist.autoConfigureDistance = false;
            dist.distance = segmentSpacing;
            dist.frequency = frequency;     
            dist.dampingRatio = dampingRatio; 
            lastSpawnPoint = spawnPoint;
          
        }

        void UpdateLineRenderer()
        {
            line.positionCount = segments.Count + 2;

            int index = 0;
            line.SetPosition(index++, anchor.position);

            foreach(var seg in segments)
                line.SetPosition(index++, seg.position);

            line.SetPosition(index++, spawnpoint);

            // Optional: tile texture based on rope length
            float totalLength = (line.positionCount - 1) * segmentSpacing;
            line.material.mainTextureScale = new Vector2(totalLength, 1);
        }
    }
}

