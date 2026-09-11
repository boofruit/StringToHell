using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using StringToHell.InGame;

public class WindVolumeController : MonoBehaviour
{
    [SerializeField] private EventReference eventRef;
    [SerializeField] private PolygonCollider2D zone;
    //[SerializeField] private Transform listener;

    [SerializeField] private float fadeDistance = 10f;

    Wind wind;

    private EventInstance instance;

    // Your gameplay volume
    private float windVolume = 1f;
    public float BaseVolume => windVolume;

    private float currentVolume = 0f;
    public float CurrentVolume => currentVolume;

    private void Awake()
    {
        instance = RuntimeManager.CreateInstance(eventRef);
        instance.start();
        instance.setVolume(0f);
        
        wind = GetComponentInChildren<Wind>();
       // if (listener == null) { listener = Camera.main.transform; }
    }


    public float GetStrength(Vector2 listenerPosition)
    {
        // Fully inside the polygon.
        if (zone.OverlapPoint(listenerPosition))
            return 1f;

        // Find the nearest point on the polygon.
        Vector2 closestPoint =
            zone.ClosestPoint(listenerPosition);

        float distance =
            Vector2.Distance(listenerPosition, closestPoint);

        return Mathf.Clamp01(
            1f - distance / fadeDistance
        );
    }

    public void SetVolume(float targetVolume, float fadeSpeed)
    {
        currentVolume = Mathf.MoveTowards(
            currentVolume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );

        instance.setVolume( currentVolume);
    }

   

    void Update()
    {
        SetGameplayVolume();
        //UpdateZoneFade();
        //UpdateBaseVolume();
    }

    //void UpdateZoneFade()
    //{
    //    Vector2 closest = zone.ClosestPoint(listener.position);

    //    float distance = zone.OverlapPoint(listener.position)
    //        ? 0f
    //        : Vector2.Distance(listener.position, closest);

    //    float zoneFade =
    //        Mathf.Clamp01(distance / fadeDistance);

    //    instance.setParameterByName("WindZoneFade", zoneFade);
    //}

    //void UpdateBaseVolume()
    //{
    //    instance.setVolume(windVolume);
    //}

    public void SetGameplayVolume()
    {
        float volume = (wind.WindForce * 2) / 100; ;
        
        windVolume = Mathf.Clamp01(volume);
    }

    void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}

