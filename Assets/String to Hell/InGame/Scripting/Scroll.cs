using UnityEngine;

namespace StringToHell.InGame
{
    public class Scroll : MonoBehaviour
    {
        //[SerializeField] GameObject follower;

        Transform tf;
        Transform playerTf;
        Vector2 dir;
        Transform camMove;
        [SerializeField] bool FollowVertical;
        [SerializeField] float speed;
        bool onPlayer = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            playerTf = GameObject.FindWithTag("Player").transform;
            //相手の座標　ー　自分の座標　＝　相手への方向

            tf = transform;


        }

        // Update is called once per frame
        void Update()
        {

            if (playerTf != null && !onPlayer)
            {

                //dir = playerTf.position - cam.transform.position;
                //camMove.Translate(dir.normalized * speed);
                var dit = playerTf.position - tf.position;
                var dity = FollowVertical ? dit.y : 0;
                dit.z = 0;
                dit.y = dity;
                tf.Translate(dit * speed * Time.deltaTime);
            }

            // cam.tf = dir;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var bumb = collision.gameObject;
            if (bumb.CompareTag("Player"))
            {
                onPlayer = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var bumb = collision.gameObject;
            if (bumb.CompareTag("Player"))
            {
                onPlayer = false;
            }
        }
    }
}
