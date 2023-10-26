using MilkShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Shaker shaker;
    [SerializeField] private ShakePreset lucid0Preset;
    [SerializeField] private ShakePreset lucid1Preset;
    [SerializeField] private ShakePreset deathPreset;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        Ability.Instance.Used += UsedAbility;
        characterHurt.Instance.onHurt.AddListener(Died);
    }
    private void OnDisable()
    {
        Ability.Instance.Used -= UsedAbility;
        characterHurt.Instance.onHurt.RemoveListener(Died);
    }
    private void UsedAbility(bool RED, bool initial)
    {
        if (initial) return;
        if (RED) Shake(lucid0Preset);
        else Shake(lucid1Preset);
    }
    private void Died()
    {
        Shake(deathPreset);
    }
    private void Shake(ShakePreset preset)
    {
        shaker.Shake(preset);
    }
}
