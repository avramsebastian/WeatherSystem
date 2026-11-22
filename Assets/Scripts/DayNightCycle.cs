using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Light & Sky References")]
    [Tooltip("Directional Light used as the Sun (falls back to RenderSettings.sun if empty)")]
    public Light sunLight;

    [Header("Time & Day-Night Cycle")]
    [Range(0f, 24f)]
    [Tooltip("Current time in 0 - 24 format (e.g., 14.5 = 14:30)")]
    public float currentHour = 14.5f;

    [Tooltip("How many in-game hours pass per real second (0 = paused / externally controlled)")]
    public float hoursPerSecond = 0.5f;

    [Tooltip("Sun rotation axis direction")]
    public Vector3 sunAxis = Vector3.right;

    [Header("Light Color & Intensity")]
    public Gradient sunColorByTime;
    public AnimationCurve sunIntensityByTime = new AnimationCurve(
        new Keyframe(0f, 0.05f),
        new Keyframe(5.5f, 0.2f),
        new Keyframe(7f, 1.5f),
        new Keyframe(12f, 2.5f),
        new Keyframe(18f, 1.5f),
        new Keyframe(20f, 0.2f),
        new Keyframe(24f, 0.05f)
    );

    public string FormattedTime
    {
        get
        {
            int totalMinutes = Mathf.FloorToInt((currentHour % 24f) * 60f);
            int hours = (totalMinutes / 60) % 24;
            int minutes = totalMinutes % 60;
            return $"{hours:00}:{minutes:00}";
        }
    }

    private void Awake()
    {
        if (sunLight == null)
        {
            sunLight = RenderSettings.sun != null ? RenderSettings.sun : FindAnyObjectByType<Light>();
        }

        if (sunColorByTime == null || sunColorByTime.colorKeys.Length == 0)
        {
            Gradient g = new Gradient();
            g.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(new Color(0.15f, 0.22f, 0.45f), 0.0f),  
                    new GradientColorKey(new Color(1.0f, 0.55f, 0.35f), 0.25f),  
                    new GradientColorKey(new Color(1.0f, 0.96f, 0.88f), 0.50f), 
                    new GradientColorKey(new Color(1.0f, 0.48f, 0.25f), 0.75f), 
                    new GradientColorKey(new Color(0.15f, 0.22f, 0.45f), 1.0f)   
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );
            sunColorByTime = g;
        }
    }

    private void Update()
    {
        if (hoursPerSecond > 0f)
        {
            currentHour = (currentHour + hoursPerSecond * Time.deltaTime) % 24f;
        }

        ApplySunState();
    }

    private void OnValidate()
    {
        currentHour = (currentHour % 24f + 24f) % 24f;
        ApplySunState();
    }

    public void SetTime(float hour)
    {
        currentHour = (hour % 24f + 24f) % 24f;
        ApplySunState();
    }

    private void ApplySunState()
    {
        if (sunLight == null) return;

        float dayProgress = currentHour / 24f; 
        float sunAngle = (currentHour - 6f) * 15f; 
        sunLight.transform.localRotation = Quaternion.Euler(sunAngle, -30f, 0f);

        sunLight.color = sunColorByTime.Evaluate(dayProgress);
        sunLight.intensity = sunIntensityByTime.Evaluate(currentHour);

        if (RenderSettings.sun == null)
        {
            RenderSettings.sun = sunLight;
        }
    }
}