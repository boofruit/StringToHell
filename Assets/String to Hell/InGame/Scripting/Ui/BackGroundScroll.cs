using UnityEngine;

public class ParallaxURP : MonoBehaviour
{
    [SerializeField] private Transform target; // player or camera
    [SerializeField] private float parallaxMultiplierX = 0.2f;
    [SerializeField] private float parallaxMultiplierY = 0.2f;

    [SerializeField] private Renderer targetRenderer;

    private Material material;
    private Vector2 baseOffset;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        // Use a unique material instance per layer
        material = targetRenderer.material;
        baseOffset = material.mainTextureOffset;
    }

    private void Update()
    {
        float offsetX = target.position.x * parallaxMultiplierX;
        float offsetY = target.position.y * parallaxMultiplierY;

        material.mainTextureOffset = baseOffset + new Vector2(offsetX, offsetY);
    }
}
