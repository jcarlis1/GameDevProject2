using UnityEngine;

public class FullNightCycle : MonoBehaviour
{
    [Header("Skybox Settings")]
    public Material skyboxMaterial;
    public float duskExposure = 1.0f;
    public float nightExposure = 0.15f;

    [Header("Directional Light (Sun/Moon)")]
    public Light mainLight;
    public float duskLightIntensity = 0.8f;
    public float nightLightIntensity = 0.05f;
    public Color duskLightColor = new Color(1.0f, 0.6f, 0.4f);  // Warm dusk orange
    public Color nightLightColor = new Color(0.1f, 0.15f, 0.3f); // Cold night blue


    [Header("Timing")]
    public float transitionDuration = 30f; // Seconds from dusk to night

    private float timer = 0f;

    void Update()
    {
        if (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / transitionDuration);

            // 1. Update Skybox Exposure
            if (skyboxMaterial != null)
            {
                float exposure = Mathf.Lerp(duskExposure, nightExposure, progress);
                skyboxMaterial.SetFloat("_Exposure", exposure);
            }

            // 2. Update Directional Light
            if (mainLight != null)
            {
                mainLight.intensity = Mathf.Lerp(duskLightIntensity, nightLightIntensity, progress);
                mainLight.color = Color.Lerp(duskLightColor, nightLightColor, progress);
            }

            // Force Unity to recalculate scene ambient reflections from the skybox
            DynamicGI.UpdateEnvironment();
        }
    }

    void OnDisable()
    {
        // Revert material back to initial state when exiting Play mode
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", duskExposure);
        }
    }
}