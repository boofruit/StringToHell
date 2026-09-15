using UnityEngine;


namespace StringToHell.InGame
{
    public class WindPolygonSpawner : MonoBehaviour
    {
        ParticleSystem ps;
         PolygonCollider2D polygon;

        [Header("Spawn Settings")]
        [SerializeField] float particlesPerSecond = 20;

        [Header("Particle Appearance")]
        [SerializeField] float particleSize = 0.1f;
        [SerializeField] float densityModifier = 0.3f;
        [SerializeField] float streakLength = 2f;
        [SerializeField] float SpeedModifier = 100f;
        [Header("Wind Settings")]
        Vector2 windDirection;
         float windSpeed = 2f;
        float area ;
        float timer;
        Wind wind;
        float accumulator = 0f;
        private void Start()
        {
        ps = GetComponent<ParticleSystem>();
        polygon = GetComponent<PolygonCollider2D>();
            area = PolygonArea(polygon);
            wind = GetComponentInChildren<Wind>();
            windDirection = wind.WindDirection;
            windSpeed = Mathf.Clamp((wind.WindForce / SpeedModifier), 0f, 1f);
            particlesPerSecond = densityModifier * area * Mathf.Max(windSpeed, 0.1f);
        }
        void Update()
        {
            windDirection = wind.WindDirection;
            if (windSpeed != wind.WindForce /SpeedModifier){
                windSpeed = wind.WindForce / SpeedModifier;
                particlesPerSecond = densityModifier * area * Mathf.Max(windSpeed, 0.1f);
            }
            timer += Time.deltaTime;
            // k controls visual density
            
            float accumulator = 0f;
                float pps = particlesPerSecond;
                accumulator += pps * Time.deltaTime;

                int emitCount = Mathf.FloorToInt(accumulator);
                if (emitCount > 0)
                {
                    accumulator -= emitCount;
                    for (int i = 0; i < emitCount; i++)
                        EmitParticle();
                }
            var main = ps.main;
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
            int count = ps.GetParticles(particles);

            for (int i = 0; i < count; i++)
            {
                if (!polygon.OverlapPoint(particles[i].position))
                    particles[i].remainingLifetime = 0;
            }

            ps.SetParticles(particles, count);

        }

        void EmitParticle()
        {
            Vector2 spawnPos = RandomPointInPolygon(polygon);

            ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
            ep.position = spawnPos;

            // Smooth, controllable wind
            Vector2 baseVel = windDirection.normalized * windSpeed;

            // Add slight randomness to avoid clumping
            Vector2 jitter = Random.insideUnitCircle * 0.2f;

            ep.velocity = baseVel + jitter;

            ep.startSize = particleSize;

            ps.Emit(ep, 1);
        }
            float PolygonArea(PolygonCollider2D poly)
        {
            float area = 0f;
            var pts = poly.points;

            for (int i = 0; i < pts.Length; i++)
            {
                Vector2 a = pts[i];
                Vector2 b = pts[(i + 1) % pts.Length];
                area += (a.x * b.y - b.x * a.y);
            }

            return Mathf.Abs(area) * 0.5f;
        }
        Vector2 RandomPointInPolygon(PolygonCollider2D poly)
        {
            // Get local-space points
            var pts = poly.points;

            // Pick a random triangle inside the polygon (simple fan triangulation)
            int i = Random.Range(1, pts.Length - 1);
            Vector2 a = pts[0];
            Vector2 b = pts[i];
            Vector2 c = pts[i + 1];

            // Random barycentric coordinates
            float r1 = Random.value;
            float r2 = Random.value;
            if (r1 + r2 > 1f)
            {
                r1 = 1f - r1;
                r2 = 1f - r2;
            }

            Vector2 localP = a + r1 * (b - a) + r2 * (c - a);

            // Convert to world space
            Vector2 worldP = poly.transform.TransformPoint(localP);
            return worldP;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (polygon == null) return;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < 50; i++)
            {
                Vector2 p = RandomPointInPolygon(polygon);
                Gizmos.DrawSphere(p, 0.05f);
            }
        }
#endif

    }
}

