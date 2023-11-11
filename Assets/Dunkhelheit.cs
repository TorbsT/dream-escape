using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dunkhelheit : MonoBehaviour
{
    public static Dunkhelheit Instance { get; private set; }
    enum Mode
    {
        Idle,
        FadeIn,
        FadeOut
    }

    [SerializeField] private Image dunkelheit;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;
    private float vis;
    private string loadToScene = "NOSCENESPECIFIED";
    Mode mode;
    public void FadeTo(string levelName)
    {
        dunkelheit.gameObject.SetActive(true);
        loadToScene = levelName;
        mode = Mode.FadeOut;
    }
    private void Awake()
    {
        if (!enabled) return;
        Instance = this;
        Debug.Log("This does not happen");
        dunkelheit.gameObject.SetActive(true);
        mode = Mode.FadeIn;
        vis = 1f;
    }
    private void Update()
    {
        if (mode == Mode.FadeIn)
        {
            if (vis < 0f)
            {
                mode = Mode.Idle;
                vis = 0f;
                dunkelheit.gameObject.SetActive(false);
                return;
            }
            vis -= Time.deltaTime/fadeInTime;
        }
        else if (mode == Mode.FadeOut)
        {
            if (vis > 1f)
            {
                SceneManager.LoadScene(loadToScene);
                enabled = false;
                return;
            }
            vis += Time.deltaTime/fadeOutTime;
        }
        dunkelheit.color = new(0f, 0f, 0f, vis);
    }
}
