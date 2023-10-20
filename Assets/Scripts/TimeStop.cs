using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    public static TimeStop Instance { get; private set; }

    [field: SerializeField] public float EaseInDuration { get; private set; } = 0.5f;
    [field: SerializeField] public AnimationCurve EaseInCurve { get; private set; } = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField] private float timeLeft;
    public void Stop(float duration)
    {
        timeLeft = duration+EaseInDuration;
        SetTimeScale(0f);
        //TODO
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        timeLeft -= Time.unscaledDeltaTime;
        if (timeLeft <= 0f)
            SetTimeScale(1f);
        else if (timeLeft <= EaseInDuration)
        {
            float progress = 1f - (timeLeft / EaseInDuration);
            float scale = EaseInCurve.Evaluate(progress);
            SetTimeScale(scale);
        }
    }
    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
