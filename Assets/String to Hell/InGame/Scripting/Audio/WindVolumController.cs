using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class WindVolumeController : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private PolygonCollider2D zone;

    [Header("Listener")]
    [SerializeField] private Transform listener;

    [Header("Audio")]
    [SerializeField] private EventReference audioEvent;

    [Header("Fade")]
    [SerializeField] private float fadeDistance = 10f;

    private EventInstance instance;

    private void Start()
    {
        instance = RuntimeManager.CreateInstance(audioEvent);
        instance.start();
    }

    private void Update()
    {
        Vector2 listenerPosition = listener.position;

        // Closest point on/in the polygon
        Vector2 closestPoint = zone.ClosestPoint(listenerPosition);

        // Distance to polygon boundary
        float distance = Vector2.Distance(listenerPosition, closestPoint);

        // Inside the polygon = full volume
        bool inside = zone.OverlapPoint(listenerPosition);

        float volume;

        if (inside)
        {
            volume = 1f;
        }
        else
        {
            // Fade from 1 -> 0 as we move away
            volume = 1f - Mathf.Clamp01(distance / fadeDistance);
        }

        instance.setParameterByName("ZoneVolume", volume);
    }

    private void OnDestroy()
    {
       // instance.stop(STOP_MODE.IMMEDIATE);
        instance.release();
    }
}
