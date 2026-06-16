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
        float spacing;


        void Start()
        {
            anchor = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            UpdateLineRenderer();
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
            segments.Add(spawner.transform);
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
            spacing = segmentSpacing;
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
        public void BungieSling(float slingForce)
        {
            if (isUnwinding || lineExtinguished) return;
            Rigidbody2D rb = BaseJoint.GetComponent<Rigidbody2D>();
            int SegementsPower = segments.Count -1;
            // Convert anchor to world space
            Vector2 worldAnchor = BaseJoint.transform.TransformPoint(BaseJoint.anchor);

            // Direction from player to anchor
            Vector2 direction = worldAnchor - anchor.position;

            float distance = direction.magnitude;

            // Normalize direction so it only represents direction, not magnitude
            Vector2 normalizedDirection = direction.normalized *-1;

            // How far past the rest length (spacing) the spring is stretched
            float stretch = (distance / SegementsPower) - spacing ;

            if (stretch <= 0f)
                return; // No tension, no force

            // Scale force by stretch amount
            float finalForce = slingForce * stretch;

            rb.AddForce(normalizedDirection * finalForce, ForceMode2D.Impulse);
        }
        public void CutThread()
        {
            if (segments == null) return;
            segments.Remove(spawner.transform);
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

        public void UpdateLineRenderer()
        {
            if (segments == null) return;
            line.positionCount = segments.Count;
            int index = 0;
            foreach (var seg in segments)
            {
                line.SetPosition(index++, seg.position);
            }
            // Optional: tile texture based on rope length
            float totalLength = (line.positionCount) * spacing;
            line.material.mainTextureScale = new Vector2(totalLength, 1);
        }
    }
}

