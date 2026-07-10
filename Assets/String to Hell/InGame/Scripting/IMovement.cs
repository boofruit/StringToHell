using System.Collections;
using UnityEngine;

namespace StringToHell.InGame
{
    public interface IMovement
    {
        bool Jumping { get; }

        //  Vector2 SurfaceNormal { get; }

        void AirMovement(Vector2 controllerInput, float airSpeed);
        Vector2 JumpDirection(Vector2 controllerInput);
        void WallMovement(Vector2 controllerInput, float moveSpeed, float pullStrength);

        void Jump(Vector2 direction, float jumpPower, float iceSlipperiness);
        void Float(Vector2 diveDirection, Vector2 inputDirection, float divePower);
        void Dive(Vector2 diveDirection, Vector2 inputDirection, float divePower, float windMultiplier);
        IEnumerator MidJump(float waitTime);
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