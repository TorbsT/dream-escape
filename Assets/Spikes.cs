using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Spikes : MonoBehaviour
{
    [field: SerializeField, Range(0, 60)] private int Count { get; set; } = 3;
    [field: SerializeField, Range(0.1f, 1f)] private float Spacing { get; set; } = 0.5f;
    [field: SerializeField] private GameObject SpikePrefab { get; set; }
    public void Refresh()
    {
        List<Transform> children = new();

        foreach (var t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.parent != transform) continue;
            children.Add(t);
        }
        while (children.Count > Count)
        {
            GameObject go = children[^1].gameObject;
            if (Application.isPlaying) Destroy(go);
            else DestroyImmediate(go);
            children.RemoveAt(children.Count-1);
        }
        while (children.Count < Count)
        {
            Transform t = Instantiate(SpikePrefab).transform;
            t.SetParent(transform);
            children.Add(t);
        }

        for (int i = 0; i < children.Count; i++)
        {
            Transform t = children[i];
            float x = Spacing * (i - (children.Count-1f) / 2f);
            t.localPosition = new Vector2(x, 0f);
        }
    }
}
