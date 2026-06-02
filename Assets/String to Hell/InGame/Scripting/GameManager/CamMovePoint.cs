using UnityEngine;

namespace StringToHell.InGame.GameManager
{
    public class CamMovePoint : MonoBehaviour
    {
        [SerializeField] float CamSpeed = .9f;
        [SerializeField] bool FollowVertical;
        Transform tf;
        [SerializeField] Camera cam;

        void Start()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
            tf = transform;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var other = collision.gameObject;
            if (other.CompareTag("Player"))
            {
                var CamPos = cam.transform.position;
                var movePoint = tf.position; //- CamPos;

                var dity = FollowVertical ? movePoint.y : 0;
                movePoint.z = -10;
                movePoint.y = dity;
                cam.transform.position = movePoint;
                //CamPos = Vector3.Lerp(CamPos, movePoint, CamSpeed);
                Debug.Log("ì¸Ç¡ÇΩÅI");
            }

        }
    }
}
