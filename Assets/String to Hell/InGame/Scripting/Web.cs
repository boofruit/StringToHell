using UnityEngine;

namespace StringToHell.InGame
{
    public class Web : MonoBehaviour, IWeb
    {

        [SerializeField] GameObject Anchor;
        [SerializeField, Tooltip("")] float threadLength = 10;

        public GameObject PlaceAnchor(Vector2 position)
        {
            var createObj = Instantiate(Anchor, position, Quaternion.identity);
            return createObj;
        }

    }
}
