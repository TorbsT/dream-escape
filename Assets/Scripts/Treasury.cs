using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Treasury")]
[ExecuteAlways]
public class Treasury : ScriptableObject
{
    [field: SerializeField, TextArea] public string Text { get; private set; }

    private List<string> list = new();

    public string GetRandom()
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    public List<string> GetAll() => list;

    private void OnValidate()
    {
        list = new();
        foreach (var item in Text.Split("; "))
            list.Add(item);
    }
}
