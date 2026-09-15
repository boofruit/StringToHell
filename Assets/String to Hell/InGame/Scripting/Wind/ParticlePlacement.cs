using UnityEngine;

public class ParticlePlacement : MonoBehaviour
{
   
    // Returns a random point inside a PolygonCollider2D
    public static Vector2 RandomPointInPolygon(PolygonCollider2D poly)
    {
        // Pick a random bounding box point until it's inside the polygon
        Bounds b = poly.bounds;

        for (int i = 0; i < 50; i++) // safety loop
        {
            Vector2 p = new Vector2(
                Random.Range(b.min.x, b.max.x),
                Random.Range(b.min.y, b.max.y)
            );

            if (poly.OverlapPoint(p))
                return p;
        }

        return poly.bounds.center; // fallback
    }
}


