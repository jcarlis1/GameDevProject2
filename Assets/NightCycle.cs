using UnityEngine;

public class FullNightCycle : MonoBehaviour
{
    [Header("Skybox Settings")]
    public Material skyboxMaterial;
    public float duskExposure = 1.0f;
    public float nightExposure = 0.2f;

    [Header("Sun Light (Fades Out)")]
    public Light sunLight;
    public float duskSunIntensity = 0.8f;
    public Color duskSunColor = new Color(1.0f, 0.6f, 0.4f);  // Warm dusk orange

    [Header("Moon Light (Fades In)")]
    public Light moonLight;
    public float nightMoonIntensity = 0.35f;                   // Higher value keeps scene readable
    public Color nightMoonColor = new Color(0.4f, 0.6f, 1.0f); // Cold silvery blue

    [Header("Ambient Lighting (Prevents Pitch Black Shadows)")]
    public Color duskAmbientColor = new Color(0.35f, 0.3f, 0.3f);
    public Color nightAmbientColor = new Color(0.08f, 0.12f, 0.22f); // Soft blue shadow fill

    [Header("Timing")]
    public float transitionDuration = 30f;

    private float timer = 0f;

    void Update()
    {
        if (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / transitionDuration);

            // 1. Skybox Exposure
            if (skyboxMaterial != null)
            {
                skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(duskExposure, nightExposure, progress));
            }

            // 2. Fade Sun OUT
            if (sunLight != null)
            {
                sunLight.intensity = Mathf.Lerp(duskSunIntensity, 0f, progress);
                sunLight.color = duskSunColor;
            }

            // 3. Fade Moon IN
            if (moonLight != null)
            {
                moonLight.intensity = Mathf.Lerp(0f, nightMoonIntensity, progress);
                moonLight.color = nightMoonColor;
            }

            // 4. Update Ambient Light Floor
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientSkyColor = Color.Lerp(duskAmbientColor, nightAmbientColor, progress);

            DynamicGI.UpdateEnvironment();
        }
    }

    void OnDisable()
    {
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", duskExposure);
        }
    }
}