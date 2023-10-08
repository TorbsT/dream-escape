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
        timeLeft = 0.05f;
        //TODO
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        timeLeft -= Time.unscaledDeltaTime;
        if (timeLeft <= 0f) Time.timeScale = 1f;
    }

}
