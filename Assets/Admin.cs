using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    private float ppower;
    private bool admin;
    // Start is called before the first frame update
    void Start()
    {
        SetAdmin(false);
    }

    // Update is called once per frame
    void Update()
    {
        ppower -= Time.deltaTime;
        ppower = Mathf.Max(ppower, 0);
        if (Input.GetKeyDown(KeyCode.P)) ppower += 1f;
        if (ppower > 3f)
        {
            SetAdmin(!admin);
        }
    }
    private void SetAdmin(bool admin)
    {
        this.admin = admin;
        PauseMenu.Instance.RestrictLevels = !admin;
        PlaytimeCanvasManager.Instance.Display = admin;
        ppower = 0f;
    }
}
