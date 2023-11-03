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

    private Vector2 fromPos;
    private Vector2 toPos;
    private Quaternion fromRot;
    private Quaternion toRot;

    private bool powered;
    private float time;
    private bool done;
    public override void Trigger(GameObject source, bool power)
    {
        if (power)
            powerSources.Add(source);
        else
            powerSources.Remove(source);
        bool newPowered = powerSources.Count > 0;

        if (newPowered != powered)
        {
            done = false;
            time = Duration - time;
        }
        powered = newPowered;


        Refresh();
    }

    private void Awake()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        activeRot = Quaternion.Euler(0f, 0f, DegreesToRotate) * transform.localRotation;
        activePos = originalPos + DistanceToTravel;
        Refresh();
    }
    void Update()
    {
        if (done) return;

        time += Time.unscaledDeltaTime;
        if (time > Duration)
        {
            time = Duration;
        }

        SetT(time / Duration);
    }
    private void SetT(float t)
    {
        transform.localRotation = Quaternion.Lerp(fromRot, toRot, t);
        transform.localPosition = Vector2.Lerp(fromPos, toPos, t);
    }
    private void Refresh()
    {
        fromPos = powered ? originalPos : activePos;
        toPos = !powered ? originalPos : activePos;
        fromRot = powered ? originalRot : activeRot;
        toRot = !powered ? originalRot : activeRot;
    }
}
