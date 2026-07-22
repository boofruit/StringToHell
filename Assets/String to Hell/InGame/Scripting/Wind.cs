using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace StringToHell.InGame
{
    public class Wind : MonoBehaviour, IWind
    {
        [SerializeField, Tooltip("")]
        float windSpeedVariation = 0f;
        [SerializeField, Range(0, 360), Tooltip("")]
        float maxwindAngle = 0f;
        [SerializeField, Range(0, 360), Tooltip("")]
        float minwindAngle = 0f;
        [SerializeField, Tooltip("")]
        float blowDuration = 0f;
        [SerializeField, Tooltip("")]
        float pauseDuration = 0f;
        [SerializeField, Tooltip("")]
        float pauseWindSpeed = 0f;
       
        private float baseWindSpeed;

        [SerializeField, Range(0, 100), Tooltip("")]
        float windForce = 10f;

        public float WindForce => windForce;

        public Vector2 WindDirection => transform.up;
        

      
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
       
                void Start()
        {
           
            baseWindSpeed = windForce;
            if (blowDuration > 0f)
            {
                StartCoroutine(BlowWind());
            }
        }



        IEnumerator BlowWind()
        {
            float elapsedTime = 0f;
            float targetAngle = Random.Range(minwindAngle, maxwindAngle);
            float targetSpeed = Random.Range(baseWindSpeed - windSpeedVariation, baseWindSpeed + windSpeedVariation);
            while (elapsedTime < blowDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / blowDuration;
                // Lerp the wind angle and speed
                if (minwindAngle != 360f)
                {
                    float rotationYDegrees = transform.eulerAngles.y;
                    float currentRotation = transform.rotation.z;
                    float rad = targetAngle * Mathf.Deg2Rad;
                    float rotation = Mathf.Lerp(currentRotation, targetAngle, t);
                    transform.rotation = Quaternion.Euler(0, 0, rotation);

                }
                    windForce = Mathf.Lerp(windForce, targetSpeed, t);
              
                yield return null;
            }
            StartCoroutine(PauseWind());
        }
        IEnumerator PauseWind()
        {
            float elapsedTime = 0f;
            while (elapsedTime < pauseDuration)
            {
                elapsedTime += Time.deltaTime;
                windForce = pauseWindSpeed;
                yield return null;
            }
            StartCoroutine(BlowWind());
        }
    }




}
