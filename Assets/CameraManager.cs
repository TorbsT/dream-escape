using MilkShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [field: SerializeField] public ShakePreset LeverPreset { get; private set; }
    [SerializeField] private Shaker shaker;
    [SerializeField] private ShakePreset lucid0Preset;
    [SerializeField] private ShakePreset lucid1Preset;
    [SerializeField] private ShakePreset lucidFailPreset;
    [SerializeField] private ShakePreset deathPreset;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        Ability.Instance.Used += UsedAbility;
        Ability.Instance.FailedUse += FailedUse;
        characterHurt.Instance.onHurt.AddListener(Died);
    }
    private void OnDisable()
    {
        Ability.Instance.Used -= UsedAbility;
        Ability.Instance.FailedUse -= FailedUse;
        characterHurt.Instance.onHurt.RemoveListener(Died);
    }
    private void UsedAbility(bool RED, bool initial)
    {
        if (initial) return;
        if (RED) Shake(lucid0Preset);
        else Shake(lucid1Preset);
    }
    private void FailedUse()
    {
        Shake(lucidFailPreset);
    }
    private void Died()
    {
        Shake(deathPreset);
    }
    public void Shake(ShakePreset preset)
    {
        shaker.Shake(preset);
    }
}
