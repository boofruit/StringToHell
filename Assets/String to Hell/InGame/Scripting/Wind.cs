using System.Collections;
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
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            wind = GetComponent<AreaEffector2D>();
            baseWindSpeed = wind.forceMagnitude;
            if (blowDuration > 0f)
            {
                StartCoroutine(BlowWind());
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            var other = collision.gameObject;
            if (other.CompareTag("Player"))
            {
                baseWindSpeed = wind.forceMagnitude;

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
                if(minwindAngle != 0f)
                {
                wind.forceAngle = targetAngle;
                wind.forceMagnitude = Mathf.Lerp(wind.forceMagnitude, targetSpeed, t);

                }
                else { 
                wind.forceMagnitude = baseWindSpeed;
                }
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
                wind.forceMagnitude = 0;
                yield return null;
            }
            StartCoroutine(BlowWind());
        }
    }
}
