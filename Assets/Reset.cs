using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public void OnReset(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("SampleScene");
    }
}
