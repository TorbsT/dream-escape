using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowUnderConditions : TriggerReceiver
{
    [field: SerializeField] public List<GameObject> Conditions { get; private set; } = new();
    private void Awake()
    {
        SetEnabled(false);
    }
    public override void Trigger(bool power)
    {
        foreach (var condition in Conditions)
        {
            if (!condition.activeSelf) return;
        }
        SetEnabled(true);
    }
    private void SetEnabled(bool enabled)
    {
        GetComponent<TextMeshProUGUI>().enabled = enabled;
    }
}
