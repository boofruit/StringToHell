using System.Threading.Tasks;
using UnityEngine;

namespace StringToHell.InGame.GameManager
{
    public class CameraMove : MonoBehaviour
    {
        Transform tf;
        Transform playerTf;
        Camera cam;
        public Camera targetCamera;
        public GameObject objectToCheck;
        [SerializeField] float TransitionSpeed = 1;



        async void PlanesTransition() //partially written by AI
        {

            if (objectToCheck != null && targetCamera != null)
            {

                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(targetCamera);
                Renderer objectRenderer = objectToCheck.GetComponentInChildren<Renderer>();

                if (objectRenderer != null)
                {
                    if (GeometryUtility.TestPlanesAABB(planes, objectRenderer.bounds))
                    {
                        Debug.Log(objectToCheck.name + " is in the camera's field of view.");
                    }
                    else
                    {
                        MoveCam();
                        Debug.Log(objectToCheck.name + " is NOT in the camera's field of view.");
                        await Task.Delay(1000);
                    }
                }


            }
        }
        void Update()
        {
            PlanesTransition();

        }
        //public async void TransitionTime()
        //{
        //    PlanesTransition();
        //    await Task.Delay(1000);
        //}
        // Vertical screen size in world units
        float screenHeight;

        // Horizontal screen size in world units
        float screenWidth;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            objectToCheck = GameObject.FindWithTag("Player");
            tf = transform;
            playerTf = GameObject.FindWithTag("Player").transform;
            cam = Camera.main;
            screenHeight = cam.orthographicSize * 2f;
            screenWidth = screenHeight * cam.aspect;

        }
        void MoveCam()
        {
            tf.position += GetDirection();
        }
        //Vector3 GetMousePOS()
        //{
        //    Vector3 pos = Input.mousePosition;
        //    pos = Camera.main.ScreenToWorldPoint(pos);
        //    pos.z = 0;
        //    return pos;
        //}
        Vector3 GetDirection()
        {
            //calculate mouse position/vector
            Vector3 toDir = playerTf.position - tf.position;
            bool isHorizontal = Mathf.Abs(toDir.x) > screenWidth / 2;
            bool isVertical = Mathf.Abs(toDir.y) > screenHeight / 2;
            if (isHorizontal)
            {
                return (toDir.x > 0 ? Vector3.right : Vector3.left) * screenWidth;
                // if player position is calculated right direction to move is right, etc. . 
            }
            if (isVertical)
            {
                return (toDir.y > 0 ? Vector3.up : Vector3.down) * screenHeight;
            }
            return Vector3.zero;
        }
        //public void TargetSquare()
        //{
        //    Vector3 direction = GetDirection();

        //    //direction of movement
        //    var targetPosition = tf.position + direction * speed;
        //    tf.position = targetPosition;
        //    // if a square can be entered execute move
        //    var collider = Physics2D.OverlapPoint(targetPosition);
        //    if (collider)
        //    {

        //    }
        //}
    }
}
