using UnityEngine;

namespace StringToHell.InGame
{
    public interface IWeb
    {
        GameObject LastString { get; set; }
        GameObject PlaceAnchor(Vector2 position);
    }
}