using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public void Clicked()
    {
        PauseMenu.Instance.SelectLevel(gameObject);
        AudioManager.Instance.Play("uibutton");
    }
}
