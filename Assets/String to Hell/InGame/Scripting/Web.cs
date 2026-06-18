using System.Collections.Generic;
using UnityEngine;

namespace StringToHell.InGame
{
    public class Web : MonoBehaviour, IWeb
    {

        [SerializeField] GameObject Anchor;
        [SerializeField, Tooltip("")] int maxWebs = 6;
        private List<GameObject> Webs = new List<GameObject>();


        public GameObject LastString { get; set; }

        public GameObject PlaceAnchor(Vector2 position)
        {
            var createObj = Instantiate(Anchor, position, Quaternion.identity);
            Webs.Add(createObj);
            if(Webs.Count > maxWebs)
            {
                Destroy(Webs[0]);
                Webs.RemoveAt(0);
            }
            return createObj;
        }

    }
}
