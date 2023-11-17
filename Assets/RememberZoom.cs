using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberZoom : MonoBehaviour
{
    public static RememberZoom Instance { get; private set; }
    public bool SkipZoom { get; set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
