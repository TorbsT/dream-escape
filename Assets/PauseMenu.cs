using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [SerializeField] private GameObject pauseMenu;

    public void Show() => SetActive(true);
    public void Hide() => SetActive(false);
    public void SetActive(bool value) => pauseMenu.SetActive(value);
    public void SelectLevel(GameObject levelGO)
    {
        SceneManager.LoadScene(levelGO.name);
    }
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetActive(!pauseMenu.activeSelf);
    }
}
