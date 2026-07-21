using Unity.VisualScripting;
using UnityEngine;

namespace StringToHell.InGame.Scripting
{
    public class Terrain : MonoBehaviour, ITerrain
    {
        [SerializeField, Range(-1, 1), Tooltip("the percentage rate linear velocity is reduced when the spider hits a wall")] float wallStop = .5f;
        public float WallStopRate => wallStop;
        [SerializeField, Range(0, 3), Tooltip("")] float jumpCooldown = 0f;
        public float JumpCooldown => jumpCooldown;

    }
}
