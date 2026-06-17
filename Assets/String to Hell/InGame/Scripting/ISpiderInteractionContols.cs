using UnityEngine;

namespace StringToHell.InGame
{
    public interface ISpiderInteractionContols
    {
        bool Clinging { get; set; }
        Vector2 SurfaceNormal { get; }
        bool Puff { get; }
        int JumpsLeft { get; }
        Vector2 ForceDirection { get; }
        bool Grounded { get; }

        void ClingSwitch();
        void Jumpcalc(int Jmp);
    }
}