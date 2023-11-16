using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : TriggerReceiver
{
    [field: SerializeField, Range(-360f, 360f)] public float DegreesToRotate { get; private set; } = 180f;
    [field: SerializeField] public Vector2 DistanceToTravel { get; private set; }
    [field: SerializeField, Range(0f, 2f)] public float Duration { get; private set; } = 1f;

    private HashSet<GameObject> powerSources = new();
    private Vector2 originalPos;
    private Quaternion originalRot;
    private Vector2 activePos;
    private Quaternion activeRot;

    private bool powered;
    private float time;
    public override void Trigger(GameObject source, bool power)
    {
        if (power)
            powerSources.Add(source);
        else
            powerSources.Remove(source);
        bool newPowered = powerSources.Count > 0;

        powered = newPowered;
    }

    private void Awake()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        activeRot = Quaternion.Euler(0f, 0f, DegreesToRotate) * transform.localRotation;
        activePos = originalPos + DistanceToTravel;

        powered = false;
        time = 0f;
        SetT(0f);
    }
    void Update()
    {
        float d = Time.unscaledDeltaTime / Duration;
        if (powered)
            time += d;
        else
            time -= d;
        time = Mathf.Clamp(time, 0f, 1f);

        SetT(time);
    }
    private void SetT(float t)
    {
        transform.localRotation = Quaternion.Lerp(originalRot, activeRot, t);
        transform.localPosition = Vector2.Lerp(originalPos, activePos, t);
    }
}
