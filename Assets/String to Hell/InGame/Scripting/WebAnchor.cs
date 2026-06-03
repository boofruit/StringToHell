using UnityEngine;

namespace StringToHell.Test.StringTest
{
    public class WebAnchor : MonoBehaviour
    {

        [SerializeField] GameObject Anchor;
        [SerializeField, Tooltip("")] float threadLength = 10;

        public GameObject PlaceAnchor(Vector2 position)
        {
            var createObj = Instantiate(Anchor);
            createObj.transform.position = position;
            return createObj;
        }

    }
}
