using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace StringToHell.InGame
{
    public class UnwindSilk : MonoBehaviour, IUnwindSilk
    {

        Rigidbody2D anchor;
        // Where rope starts
        GameObject spawner;
        Vector2 spawnPoint;             // Where rope starts
        [SerializeField] GameObject segmentPrefab;         // Rope segment prefab
        [SerializeField] LineRenderer line;                // Visual rope

        private List<Transform> segments = new List<Transform>();
        SpringJoint2D BaseJoint;

        bool isUnwinding = false;
        bool lineExtinguished = false;
        public bool LineExtinguished => lineExtinguished;
        Vector2 lastSpawnPoint;



        void Start()
        {
            anchor = GetComponent<Rigidbody2D>();
        }
        public void StartThread(Rigidbody2D newAnchor, GameObject newSpawner, SpringJoint2D baseJoint)
        {
            anchor = newAnchor;
            spawner = newSpawner;
            BaseJoint = baseJoint;
            BaseJoint.enabled = true;
            BaseJoint.connectedBody = anchor;

            spawnPoint = spawner.transform.position;
            lastSpawnPoint = spawnPoint;
            segments.Add(anchor.transform);
            isUnwinding = true;
        }
        public void StopThread()
        {
            if(segments ==  null) return;
            segments.Add(BaseJoint.transform);
            isUnwinding = false;
        }

        public void AddSegment(Vector2 spawnPoint, float segmentSpacing, int maxSegementsLength, float frequency, float dampingRatio)
        {
            if (!isUnwinding || segments.Count >= maxSegementsLength) return;
            if ((spawnPoint - lastSpawnPoint).magnitude < segmentSpacing * 3f) return;

            Transform last = segments.LastOrDefault();

            Vector2 newPos = last.position - last.up * segmentSpacing;

            GameObject seg = Instantiate(segmentPrefab, spawnPoint, Quaternion.identity);
            segments.Add(seg.transform);
            //seg.transform.localPosition = newPos;

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
            if (segments == null) return;
            segments.Remove(BaseJoint.transform);
            BaseJoint.connectedBody = null;
            BaseJoint.enabled = false;
            lineExtinguished = true;
        }
        public void ConnectLine(GameObject WebJoint)
        {
            if (lineExtinguished) return;
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

        public void UpdateLineRenderer(float segmentSpacing)
        {
            if (segments == null) return;
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

