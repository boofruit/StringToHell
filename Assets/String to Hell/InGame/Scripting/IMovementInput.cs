using UnityEngine;

namespace StringToHell.InGame
{
    public interface IMovementInput
    {
        Vector2 Move { get; }
        float MoveMagnitude { get; }
        bool IsJump { get; }
        Vector2 IsDiving { get; }
        bool IsTriggerOn { get; }
        bool IsTriggerHold { get; }
        bool IsTriggerOff { get; }
    }
}