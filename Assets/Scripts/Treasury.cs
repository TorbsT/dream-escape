using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Treasury")]
[ExecuteAlways]
public class Treasury : ScriptableObject
{
    [field: SerializeField, TextArea] public string Text { get; private set; }

    private List<string> list = new();
    private void Awake()
    {
        list = new();
        foreach (var item in Text.Split("; "))
            list.Add(item);
    }
    public string GetRandom()
    {
        if (list.Count == 0) return "";
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    public List<string> GetAll() => list;
}
