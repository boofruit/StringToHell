using System.Collections.Generic;
using StringToHell.InGame;
using UnityEngine;

public class AudioManager_WindZones : MonoBehaviour
{
     private Transform listener;

    [Header("Zone Search")]
    [SerializeField] private float searchDistance = 20f;
    [SerializeField] private LayerMask Wind;

    [Header("Crossfade")]
    [SerializeField] private float crossfadeSpeed = 1f;

    private Collider2D[] nearbyColliders = new Collider2D[64];
    private ContactFilter2D zoneFilter;

    private WindVolumeController currentZone;
    private WindVolumeController previousZone;

    private HashSet<WindVolumeController> nearbyZones = new HashSet<WindVolumeController>();
    //private float currentTargetVolume;
    //private float previousTargetVolume;

    private void Awake()
    {
        listener = GetComponent<Transform>();
        zoneFilter = new ContactFilter2D();
        zoneFilter.SetLayerMask(Wind);
        zoneFilter.useTriggers = true;
    }
    private void Update()
    {
        FindLoudestZone();
        CrossfadeZones();
    }

    private void FindLoudestZone()
    {
                    int hitCount = Physics2D.OverlapCircle(
            listener.position,
            searchDistance,
            zoneFilter,
            nearbyColliders
            );
        nearbyZones.Clear();
        WindVolumeController loudestZone = null;
        float loudestStrength = 0f;

        for (int i = 0; i < hitCount; i++)
        {
            WindVolumeController zone =
                nearbyColliders[i].GetComponent<WindVolumeController>();

            if (zone == null)
                continue;
            nearbyZones.Add(zone);
            float strength =
                zone.GetStrength(listener.position);

            if (strength > loudestStrength)
            {
                loudestStrength = strength;
                loudestZone = zone;
            }
        }

        //// No useful zone nearby.
        //if (loudestZone == null)
        //{
        //    currentTargetVolume = 0f;
        //    previousTargetVolume = 0f;
        //    return;
        //}

        // We have entered a different zone.
        if (loudestZone != currentZone)
        {
            previousZone = currentZone;
            currentZone = loudestZone;
        }

        //currentTargetVolume =
        //    loudestStrength * baseVolume;
    }

    private void CrossfadeZones()
    {
        // Fade the new zone in.
        if (currentZone != null)
        {
            float strength =
                currentZone.GetStrength(listener.position);

            float targetVolume =
                currentZone.BaseVolume * strength;

            currentZone.SetVolume(
                targetVolume,
                crossfadeSpeed
            );
        }
        // Fade the old zone out.
        if (previousZone != null)
        {
            previousZone.SetVolume(
                0f,
                crossfadeSpeed
            );

            // Once essentially silent, forget it.
            if (previousZone.CurrentVolume <= 0.001f)
            {
                previousZone = null;
            }
        }
        // Any other nearby zone stays silent.
        foreach (WindVolumeController zone in nearbyZones)
        {
            if (zone == currentZone ||
                zone == previousZone)
            {
                continue;
            }

            zone.SetVolume(
                0f,
                crossfadeSpeed
            );
        }

    }
}

