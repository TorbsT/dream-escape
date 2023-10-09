using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public static Reset Instance {  get; private set; }

    public void OnReset(InputAction.CallbackContext context)
    {
        Restart();
    }
    public void Restart()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }

    private void Awake()
    {
        Instance = this;
    }
}
