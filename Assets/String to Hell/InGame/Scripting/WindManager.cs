using System.Collections.Generic;
using UnityEngine;

namespace StringToHell.InGame
{
    public class WindManager : MonoBehaviour
    {
        private List<Wind> zones = new List<Wind>();
        private Wind current;
        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            var zone = col.GetComponentInChildren<Wind>();
            if (zone != null)
            {
                zones.Add(zone);
                UpdateCurrent();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            var zone = col.GetComponentInChildren<Wind>();
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
                if (z.windForce > current.windForce)
                    current = z;
        }

        void FixedUpdate()
        {
            if (current == null)
                return;

            // Apply ONLY the strongest wind zone's force
            rb.AddForce(current.Direction * current.windForce);
        }
    }
}
