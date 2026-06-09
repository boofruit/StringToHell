using UnityEngine;

namespace StringToHell.InGame
{
    public interface IDirectionAndRotation
    {
        void AirRotation();
        void ChangeDirection(Vector2 newDirection);
        void RotateBody(float rotationSpeed);
        void RotateInstant(Vector2 normal);
    }
}