using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public static Head Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
