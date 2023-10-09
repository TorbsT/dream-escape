using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRegister : MonoBehaviour
{
    public static PrefabRegister Instance { get; private set; }

    [field: SerializeField] public GameObject SpikePrefab { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
