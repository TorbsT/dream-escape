using MilkShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Shaker shaker;
    [SerializeField] private ShakePreset lucid0Preset;
    [SerializeField] private ShakePreset lucid1Preset;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        Ability.Instance.Used += UsedAbility;
    }
    private void OnDisable()
    {
        Ability.Instance.Used -= UsedAbility;
    }
    private void UsedAbility(bool RED, bool initial)
    {
        if (initial) return;
        if (RED) shaker.Shake(lucid0Preset);
        else shaker.Shake(lucid1Preset);
    }
}
