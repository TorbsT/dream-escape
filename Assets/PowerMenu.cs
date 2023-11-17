using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMenu : MonoBehaviour
{
    [SerializeField] private GameObject powerMenu;
    private int usesLeft = 2;
    private void OnEnable()
    {
        Ability.Instance.Used += UsedPower;
        Ability.Instance.FailedUse += FailedUse;
        Refresh();
    }
    private void OnDisable()
    {
        Ability.Instance.Used -= UsedPower;
        Ability.Instance.FailedUse -= FailedUse;
    }
    private void FailedUse()
    {
        //DialogManager.Instance.OutOfPowerUses();
    }
    private void UsedPower(bool RED, bool initial)
    {
        if (!initial) usesLeft--;
        Refresh();
    }
    private void Refresh()
    {
        int i = 0;
        foreach (var el in powerMenu.GetComponentsInChildren<Transform>())
        {
            if (el.transform.parent.parent != powerMenu.transform) continue;
            el.gameObject.SetActive(i < usesLeft);
            i++;
        }
    }
}
