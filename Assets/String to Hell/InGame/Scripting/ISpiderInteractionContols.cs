using UnityEngine;

namespace StringToHell.InGame
{
    public interface ISpiderInteractionContols
    {
        bool Clinging { get; }
        Vector2 SurfaceNormal { get; }
        bool Puff { get; }
        int JumpsLeft { get; }
        Vector2 ForceDirection { get; }

        void Jumpcalc(int Jmp);
    }
}