using UnityEngine;

namespace StringToHell.InGame
{
    public interface IWind
    {
        Vector2 WindDirection { get; }
        float WindForce { get; }
    }
}