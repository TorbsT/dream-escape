using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerMenu : MonoBehaviour
{
    [SerializeField] private GameObject powerMenu;
    [SerializeField] private TextMeshProUGUI outOfPower;
    private int usesLeft = 2;
    private float time;
    private void OnEnable()
    {
        Ability.Instance.Used += UsedPower;
        Ability.Instance.FailedUse += FailedUse;
        outOfPower.gameObject.SetActive(false);
        Refresh();
    }
    private void OnDisable()
    {
        Ability.Instance.Used -= UsedPower;
        Ability.Instance.FailedUse -= FailedUse;
    }
    private void Update()
    {
        if (time < 0) return;
        time -= Time.deltaTime;
        if (time < 0f)
        {
            outOfPower.gameObject.SetActive(false);
        } else
        {
            float opacity = time < 1f ? time : 1f;
            outOfPower.color = new(outOfPower.color.r, outOfPower.color.g, outOfPower.color.b, opacity);
        }
    }
    private void FailedUse()
    {
        time = 5f;
        outOfPower.gameObject.SetActive(true);
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
