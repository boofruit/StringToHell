using System.Collections.Generic;
using UnityEngine;

namespace StringToHell.InGame
{
    public class WindManager : MonoBehaviour
    {
        private List<IWind> zones = new List<IWind>();
        private IWind current;
        private Rigidbody2D rb;
        public float maxWindForce = 0;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            var zone = col.GetComponentInChildren<IWind>();
            if (zone != null)
            {
                zones.Add(zone);
                UpdateCurrent();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            var zone = col.GetComponentInChildren<IWind>();
            if (zone != null)
            {
                zones.Remove(zone);
                UpdateCurrent();
            }
        }

        void UpdateCurrent()
        {
            if (zones.Count == 0)
            {
                current = null;
                return;
            }

            // Pick the strongest wind zone
            current = zones[0];
            foreach (var z in zones)
                if (z.WindForce > current.WindForce)
                    current = z;
        }

        void FixedUpdate()
        {
            if (current == null)
                return;
            float finalForce = maxWindForce == 0f ? current.WindForce : Mathf.Clamp(current.WindForce, 0f, maxWindForce);
            // Apply ONLY the strongest wind zone's force
            rb.AddForce(current.WindDirection * current.WindForce);
        }
    }
}
