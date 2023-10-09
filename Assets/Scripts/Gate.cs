using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : TriggerReceiver
{
    [field: SerializeField, Range(-360f, 360f)] public float DegreesToRotate { get; private set; } = 180f;
    [field: SerializeField, Range(0f, 2f)] public float Duration { get; private set; } = 1f;
    [field: SerializeField] public bool Active { get; private set; } = false;

    private bool rotating;
    public override void Trigger(bool power)
    {
        StartCoroutine(Rotate(power ? DegreesToRotate : -DegreesToRotate, Duration));
    }
    IEnumerator Rotate(float degs, float duration)
    {
        rotating = true;

        Quaternion originalRot = transform.localRotation;
        Quaternion newRot = Quaternion.Euler(0f, 0f, degs) * transform.localRotation;

        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.unscaledDeltaTime / duration;
            transform.localRotation = Quaternion.Lerp(originalRot, newRot, t);
            yield return null;
        }

        rotating = false;
    }
}
