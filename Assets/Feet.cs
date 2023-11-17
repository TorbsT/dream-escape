using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public static Feet Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
