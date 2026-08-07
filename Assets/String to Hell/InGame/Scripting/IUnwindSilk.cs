using UnityEngine;

namespace StringToHell.InGame
{
    public interface IUnwindSilk
    {
        bool LineConnected { get; }
        bool Tugging { get; }
        Vector2 SlingDirection { get; }
        bool IsUnwinding { get; }

        void AddSegment(int maxSegementsLength, float frequency, float dampingRatio, float spacingMultiplier);
        void BungieSling();
        void CalculateStrech(float slingForce, float minTension, float maxPower);
        void ConnectLine(GameObject WebJoint, float frequency, float dampingRatio);
        void CutThread();
        void Extinguish();
        void StartThread(Rigidbody2D newAnchor, float spacing);
        void StopThread();
      
    }
}