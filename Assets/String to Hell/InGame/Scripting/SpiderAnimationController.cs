
using UnityEngine;
namespace StringToHell.InGame
{
    public class SpiderAnimationController : MonoBehaviour
    {
        ISpiderInteractionContols spiderPosition;
        IUnwindSilk silk;
        IDirectionAndRotation RotationControls;
        IMovement movement;
        IMovementInput input;
        IVelocityController velocityController;
        Animator spiderAnime;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spiderAnime = GetComponent<Animator>();
            silk = GetComponentInChildren<IUnwindSilk>();
            spiderPosition = GetComponent<ISpiderInteractionContols>();
            RotationControls = GetComponent<IDirectionAndRotation>();
            movement = GetComponent<IMovement>();
            input = GetComponent<IMovementInput>();
            velocityController = GetComponent<IVelocityController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (input.Move != Vector2.zero && spiderPosition.Clingable)
            {
                spiderAnime.SetTrigger("Walk");
            }
            if (spiderPosition.Clinging)
            {
                spiderAnime.SetBool("Cling", true);
            }
            else { spiderAnime.SetBool("Cling", false); }
        }
    }
}
