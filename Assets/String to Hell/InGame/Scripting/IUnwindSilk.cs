using UnityEngine;

namespace StringToHell.InGame
{
    public interface IUnwindSilk
    {
        bool LineExtinguished { get; }

        void AddSegment(int maxSegementsLength, float frequency, float dampingRatio);
        void BungieSling(float slingForce);
        void ConnectLine(GameObject WebJoint);
        void CutThread();
        void StartThread(Rigidbody2D newAnchor, SpringJoint2D baseJoint, float spacing);
        void StopThread();
      
    }
}