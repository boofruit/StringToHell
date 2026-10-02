using UnityEngine;

namespace StringToHell.InGame
{
    public class PlatformMover : MonoBehaviour
    {
        [Header("‚ä‚ê‚Ì‘¬“x(•‰‚Ì’l‚Å‹t‰ñ“])")]
        [SerializeField] float Xspeed;
        [SerializeField] float Yspeed;
        [Header("‚ä‚ê‚Ì‘å‚«‚³")]
        [SerializeField] private float amplitudeX = 0.5f;
        [SerializeField] private float amplitudeY = 0.5f;
        [Header("“®‚­•ûŒü(XY‚Å‰~‰^“®)")]
        [SerializeField] private bool movableX = false;
        [SerializeField] private bool movableY = true;
        Transform tf;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tf = transform;
        }

        // Update is called once per frame
        void Update()
        {
            float x = Mathf.Cos(Time.time * Xspeed) * amplitudeX;
            float y = Mathf.Sin(Time.time * Yspeed) * amplitudeY;
            float moveX = movableX ? x : 0;
            float moveY = movableY ? y : 0;
            var move = new Vector3(moveX, moveY, 0);
            tf.Translate(move * Time.deltaTime, Space.World);
        }
    }
}
