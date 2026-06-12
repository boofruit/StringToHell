using UnityEngine;

namespace StringToHell.InGame
{
    public interface IUnwindSilk
    {
        bool LineExtinguished { get; }

        void AddSegment(Vector2 spawnPoint, float segmentSpacing, int maxSegementsLength, float frequency, float dampingRatio);
        void ConnectLine(GameObject WebJoint);
        void CutThread();
        void StartThread(Rigidbody2D newAnchor, GameObject newSpawner, SpringJoint2D baseJoint);
        void StopThread();
        void UpdateLineRenderer(float segmentSpacing);
    }
}