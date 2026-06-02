using UnityEngine;

namespace StringToHell.InGame
{

    public class VelocityController : MonoBehaviour, IVelocityController
    {
        Rigidbody2D TargetRB;
        bool needUpdateVelocity = false;
        Vector2 currentVelocity = Vector2.zero;
        [SerializeField]
        bool inheritRbVelocity = true;
        public Vector2 CurrentVelocity
        {
            get
            {
                if (needUpdateVelocity && inheritRbVelocity)
                {
                    currentVelocity = TargetRB.linearVelocity;
                    needUpdateVelocity = false;
                }
                return currentVelocity;
            }
            set
            {
                needUpdateVelocity = false;
                currentVelocity = value;
            }
        }


        void Awake()
        {
            TargetRB = GetComponent<Rigidbody2D>();
        }


        void FixedUpdate()
        {
            if (needUpdateVelocity && inheritRbVelocity)
            {
                currentVelocity = TargetRB.linearVelocity;
            }
            else
            {
                TargetRB.linearVelocity = currentVelocity;
            }
            needUpdateVelocity = true;
        }



    }
}
