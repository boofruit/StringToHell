using System.Collections;
using UnityEngine;

namespace StringToHell.InGame.GameManager
{
    public class CheckPointController : MonoBehaviour
    {
        IVelocityController velocityController;
        [SerializeField, Tooltip("")] string[] CheckpointTags;
        [SerializeField, Tooltip("")] string[] ReloadTags;
        TagCheck tagC;
        Animator animator;
       public Menu menu;
       
        public Vector3 checkPoint;
        [SerializeField] GameObject StartCheckpoint;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            tagC = GetComponent<TagCheck>();
            velocityController = GetComponent<IVelocityController>();
            if (checkPoint == Vector3.zero)
            {
                checkPoint = StartCheckpoint.transform.position;
            }
        }

        public void menuTeleport()
        {
            StartCoroutine(Teleport());
        }
         IEnumerator Teleport()
        {
            animator.Play("Death");
            // Wait for the transition to end
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

            menu.PauseTime();
            // Do some action

            // Wait for the animation to end
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
            menu.PauseTime();
            transform.position = checkPoint;
            velocityController.SpiderReset();
            Debug.Log("Reloading to Checkpoint: " + checkPoint);
           
        }
        //when a checkpoint is reached record it,
        //& enable a map ui button for said object which teleports you to said check point or scene
        void SaveAndEnableCheckpoint()
        {

        }
        //saves last checkpoint as spawn/awake position of character
        void LastCheckpoint()
        {
            //PlayerPrefs.Set
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            var onEnter = collision.gameObject;
            if (tagC.CheckTags( CheckpointTags, onEnter.tag))
            {
                checkPoint = onEnter.transform.position;
                Debug.Log("Checkpoint Reached: " + checkPoint);

                //var reloads = FindObjectsByType<Reload>(FindObjectsSortMode.None); foreach (var r in reloads) { r.checkPoint = gameObject; }
            }
            if (tagC.CheckTags( ReloadTags, onEnter.tag))
            {
                StartCoroutine( Teleport());
               
            }
        }
    }
}
