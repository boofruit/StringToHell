using UnityEngine;
namespace StringToHell.InGame
{
    public class JointManager : MonoBehaviour
    {
        IUnwindSilk silk;
        private void OnJointBreak2D(Joint2D joint)
        {
            //if( joint.enabled ) {return; }
            joint.enabled = false;
            joint.connectedBody = null;
            silk.Extinguish();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            silk = GetComponentInChildren<IUnwindSilk>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}