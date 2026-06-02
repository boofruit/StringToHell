using UnityEngine;

namespace StringToHell.InGame
{
    public interface ISpiderInteractionContols
    {
        bool Clinging { get; }
        Vector2 SurfaceNormal { get; }

        void Dive(float divePower, float windMultiplier);
        void Jump(Vector2 direction, float jumpPower);
    }
}