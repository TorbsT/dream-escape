using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Image lockImg;
    [SerializeField] private TextMeshProUGUI field;
    [SerializeField] private Button button;
    public void SetEnabled(bool enabled)
    {
        field.enabled = enabled;
        button.interactable = enabled;
        lockImg.enabled = !enabled;
    }
    public void Clicked()
    {
        AudioManager.Instance.Play("uibutton");
        PauseMenu.Instance.SelectLevel(gameObject);
    }
}
