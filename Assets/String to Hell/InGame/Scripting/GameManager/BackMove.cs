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
        [SerializeField] bool FollowHorizontal = false;
        Vector3 LastPos = Vector3.zero;
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
            if (LastPos == Vector3.zero)
            {
                LastPos = pos;
                return;
            }
            Vector2 dir = pos - LastPos;
            if (!FollowHorizontal)
            {
                dir.x = 0;
            }
            else
            {
                dir.x = dir.x * Xrate;
            }
           
            if (FollowVertical)
            {
                dir.y = dir.y * Yrate;
            }
            else
            {
                dir.y = 0;
            }
           
          
            tf.Translate(dir);
            LastPos = pos;
        }
    }
}
