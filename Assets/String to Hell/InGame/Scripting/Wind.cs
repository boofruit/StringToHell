using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace StringToHell.InGame
{
    public class Wind : MonoBehaviour
    {
        [SerializeField,Tooltip("")]
        float windSpeedVariation = 0f;
        [SerializeField,Range(0, 360), Tooltip("")]
        float maxwindAngle = 0f;
        [SerializeField,Range(0, 360), Tooltip("")]
        float minwindAngle = 0f;
        [SerializeField, Tooltip("")]
        float blowDuration = 0f;
        [SerializeField, Tooltip("")]
        float pauseDuration = 0f;
        [SerializeField, Tooltip("")]
        float pauseWindSpeed = 0f;
        AreaEffector2D wind;
        private float baseWindSpeed;

        public float windForce = 10f;

        public Vector2 Direction => transform.up;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Vector3 start = transform.position;
            Vector3 end = start + transform.up * 1.5f;

            Gizmos.DrawLine(start, end);

            Vector3 right = Quaternion.Euler(0, 0, 20) * (-transform.up);
            Vector3 left = Quaternion.Euler(0, 0, -20) * (-transform.up);

            Gizmos.DrawLine(end, end + right * 0.4f);
            Gizmos.DrawLine(end, end + left * 0.4f);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
//        void Start()
//        {
//            wind = GetComponent<AreaEffector2D>();
//            baseWindSpeed = wind.forceMagnitude;
//            if (blowDuration > 0f)
//            {
//                StartCoroutine(BlowWind());
//            }
//        }



//        IEnumerator BlowWind()
//        {
//            float elapsedTime = 0f;
//            float targetAngle = Random.Range(minwindAngle, maxwindAngle);
//            float targetSpeed = Random.Range(baseWindSpeed - windSpeedVariation, baseWindSpeed + windSpeedVariation);
//            while (elapsedTime < blowDuration)
//            {
//                elapsedTime += Time.deltaTime;
//                float t = elapsedTime / blowDuration;
//                // Lerp the wind angle and speed
//                if(minwindAngle != 0f)
//                {
//                wind.forceAngle = targetAngle;
//                wind.forceMagnitude = Mathf.Lerp(wind.forceMagnitude, targetSpeed, t);

//                }
//                else { 
//                wind.forceMagnitude = baseWindSpeed;
//                }
//                yield return null;
//            }
//            StartCoroutine(PauseWind());
//        }
//        IEnumerator PauseWind()
//        {
//            float elapsedTime = 0f;
//            while (elapsedTime < pauseDuration)
//            {
//                elapsedTime += Time.deltaTime;
//                wind.forceMagnitude = pauseWindSpeed;
//                yield return null;
//            }
//            StartCoroutine(BlowWind());
//        }
    }
    



}
