using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucidMask : MonoBehaviour
{

    [SerializeField] private AnimationCurve dispandAnimCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve expandCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float expandAnimDuration = 0.99f;
    [SerializeField] private float dispandAnimDuration = 1.465f;
    [SerializeField] private float animStartSize = 0.01f;
    [SerializeField] private float animEndSize = 500f;
    private bool active;
    private float age;
    private bool expanding;

    private void OnEnable()
    {
        Ability.Instance.Used += Boom;
    }
    private void OnDisable()
    {
        Ability.Instance.Used -= Boom;
    }
    private void Update()
    {
        transform.position = Head.Instance.transform.position;
        if (!active) return;

        if (expanding)
        {
            age += Time.unscaledDeltaTime;
            if (age > expandAnimDuration)
            {
                transform.localScale = Vector3.one * animEndSize;
                active = false;
                return;
            }
            transform.localScale = Vector3.one * (animEndSize * expandCurve.Evaluate(age / expandAnimDuration) + animStartSize);
        } else
        {
            age -= Time.unscaledDeltaTime;
            if (age < 0f)
            {
                transform.localScale = Vector3.one * animStartSize;
                active = false;
                transform.position = Vector3.left * 100f;
                return;
            }
            transform.localScale = Vector3.one * (animEndSize * dispandAnimCurve.Evaluate(age / dispandAnimDuration) + animStartSize);
        }
    }
    private void Boom(bool RED, bool initial)
    {
        transform.localScale = Vector3.one * animStartSize;
        transform.position = Ability.Instance.transform.position;
        if (initial)
        {
            active = false;
            return;
        }

        age = 0f;
        active = true;
        expanding = !RED;
        if (!expanding)
            age = dispandAnimDuration;
    }
}
