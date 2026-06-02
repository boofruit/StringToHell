using UnityEngine;

namespace StringToHell.InGame
{
    public interface IMovement
    {
      //  Vector2 SurfaceNormal { get; }

        void AirMovement(Vector2 controllerInput, float airSpeed);
        void ChangeDirection(Vector2 newDirection);
        Vector2 JumpDirection(Vector2 controllerInput);
        void RotateBody(float rotationSpeed);
        void RotateInstant(Vector2 normal);
        void WallMovement(Vector2 controllerInput, float moveSpeed);
    }
    public readonly struct MovementParameter
    {
        public readonly float speed;
        public readonly float airSpeed;
        public MovementParameter(float speed, float airSpeed)
        {
            this.speed = speed;
            this.airSpeed = airSpeed;
        }
    }
    //public interface IMovement
    //{
    //    void Move(Vector2 value, float time, in MovementParameter parameter);

    //}
}