using UnityEngine;

namespace StringToHell.InGame
{
    public interface IMovementInput
    {
        Vector2 Move { get; }
        float MoveMagnitude { get; }
        bool IsJump { get; }
        Vector2 IsDiving { get; }
        public bool IsSpinnerOn { get; }
        public bool IsSpinnerHold { get; }
        public bool IsSpinnerOff { get; }
        bool IsCutWeb { get; }
        bool IsRewindString { get; }
        bool IsGrab { get; }
    }
}