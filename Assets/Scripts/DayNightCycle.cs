using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sunLight;
    [SerializeField, Range(0f, 24f)] private float currentHour = 14.5f;
    [SerializeField] private float hoursPerSecond = 0.5f;
    [SerializeField] private Vector3 sunAxis = Vector3.right;
    [SerializeField] private Gradient sunColorByTime;
    [SerializeField] private AnimationCurve sunIntensityByTime = new AnimationCurve(
        new Keyframe(0f, 0.05f),
        new Keyframe(5.5f, 0.2f),
        new Keyframe(7f, 1.5f),
        new Keyframe(12f, 2.5f),
        new Keyframe(18f, 1.5f),
        new Keyframe(20f, 0.2f),
        new Keyframe(24f, 0.05f)
    );

    public float CurrentHour => currentHour;

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
            Gradient gradient = new Gradient();
            gradient.SetKeys(
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
            sunColorByTime = gradient;
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
