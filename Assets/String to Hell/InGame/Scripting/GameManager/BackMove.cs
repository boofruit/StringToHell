using StringToHell.InGame;
using UnityEngine;

namespace StringToHell.InGame.GameManager
{
    public class BackMove : MonoBehaviour
    {
        [SerializeField] float Xrate = .5f;
        [SerializeField] float Yrate = .2f;
        Transform tf;
        Transform camTf;
        [SerializeField] bool FollowVertical = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tf = GetComponent<Transform>();
            var pl = FindAnyObjectByType<SpiderInteractionContols>();
            camTf = pl.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            var pos = camTf.position;
            pos.x = pos.x * Xrate;
            if (FollowVertical)
            {
                pos.y = pos.y * Yrate;
            }
            else
            {
                pos.y = 0;
            }
            pos.z = 0;
            tf.position = pos;
        }
    }
}
