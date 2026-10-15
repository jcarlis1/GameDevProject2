using UnityEngine;

public class PulsingPad : MonoBehaviour
{
    [Header("Pulse Controls")]
    public float pulseSpeed = 3.0f; // Speed of the pulsing loop

    [Header("1. Material Glow (Emission)")]
    public Renderer padRenderer;
    [ColorUsage(true, true)] // Enables HDR color picker in Inspector
    public Color orangeGlowColor = new Color(1f, 0.4f, 0f);
    public float minGlow = 0.5f;
    public float maxGlow = 3.0f;


    [Header("2. Scale Pulse (Breathing Effect)")]
    public bool enableScalePulse = true;
    public float scaleAmount = 0.05f; // 5% growth

    private Vector3 originalScale;
    private Material padMaterial;

    private void Start()
    {
        originalScale = transform.localScale;

        // Auto-get renderer if not manually assigned
        if (padRenderer == null)
        {
            padRenderer = GetComponent<Renderer>();
        }

        if (padRenderer != null)
        {
            // Creates a unique instance of the material so other orange objects don't pulse
            padMaterial = padRenderer.material;
            padMaterial.EnableKeyword("_EMISSION");
        }

    
    }

    private void Update()
    {
        // Smooth 0 to 1 cycle using Sine wave
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // 1. Animate Material Emission Intensity
        if (padMaterial != null)
        {
            float currentIntensity = Mathf.Lerp(minGlow, maxGlow, t);
            padMaterial.SetColor("_EmissionColor", orangeGlowColor * currentIntensity);
        }


        // 2. Animate Object Scale (Gentle Breathing)
        if (enableScalePulse)
        {
            float scaleFactor = 1f + (Mathf.Sin(Time.time * pulseSpeed) * scaleAmount);
            transform.localScale = originalScale * scaleFactor;
        }
    }
}