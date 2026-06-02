using UnityEngine;

namespace StringToHell.InGame
{
    public interface IVelocityController
    {
        Vector2 CurrentVelocity { get; set; }
    }
}