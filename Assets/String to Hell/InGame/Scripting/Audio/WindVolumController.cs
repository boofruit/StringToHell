using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using StringToHell.InGame;

public class PolygonZoneAudio : MonoBehaviour
{
    [SerializeField] private EventReference eventRef;
    [SerializeField] private PolygonCollider2D zone;
    [SerializeField] private Transform listener;

    [SerializeField] private float fadeDistance = 10f;

    Wind wind;

    private EventInstance instance;

    // Your gameplay volume
    private float baseVolume = 1f;

    void Start()
    {
        wind = GetComponentInChildren<Wind>();
        instance = RuntimeManager.CreateInstance(eventRef);
        instance.start();
    }

    void Update()
    {
        SetGameplayVolume();
        UpdateZoneFade();
        UpdateBaseVolume();
    }

    void UpdateZoneFade()
    {
        Vector2 closest = zone.ClosestPoint(listener.position);

        float distance = zone.OverlapPoint(listener.position)
            ? 0f
            : Vector2.Distance(listener.position, closest);

        float zoneFade =
            Mathf.Clamp01(distance / fadeDistance);

        instance.setParameterByName("WindZoneFade", zoneFade);
    }

    void UpdateBaseVolume()
    {
        instance.setVolume(baseVolume);
    }

    public void SetGameplayVolume()
    {
        float volume = (wind.WindForce ) / 100; ;
        
        baseVolume = Mathf.Clamp01(volume);
    }

    void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}

