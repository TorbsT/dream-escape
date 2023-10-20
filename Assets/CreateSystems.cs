using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSystems : MonoBehaviour
{
    [SerializeField] private List<GameObject> dontDestroys = new();
    [SerializeField] private List<GameObject> doDestroys = new();
    public static bool done = false;

    private void Awake()
    {
        foreach (var prefab in doDestroys)
            Instantiate(prefab);

        if (done) return;
        foreach (var prefab in dontDestroys)
        {
            var go = Instantiate(prefab);
            DontDestroyOnLoad(go);
        }

        done = true;
    }
}
