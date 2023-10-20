using System.Collections;
using System.Collections.Generic;
using TorbuTils.EzPools;
using UnityEngine;

[ExecuteAlways]
public class LumenManager : MonoBehaviour
{
    [SerializeField] private int lumensToSpawn = 100;
    [SerializeField] private float lumenAcceleration = 0.001f;
    [SerializeField] private float lumenGravity = 0.01f;
    [SerializeField] private float lumenSizeConsideration = 2f;
    [SerializeField] private float lumenMaxSpeed = 0.1f;
    [SerializeField] private GameObject lumenPrefab;
    [SerializeField] private Rect rect;
    private List<Transform> lumens = new();
    private List<Vector2> lumenVelocities = new();

    private void Start()
    {
        if (!Application.isPlaying) return;
        for (int i = 0; i < lumensToSpawn; i++)
        {
            Transform t = Pools.Instance.Depool(lumenPrefab).transform;
            t.position = new Vector2(
                Random.Range(rect.x, rect.x + rect.width),
                Random.Range(rect.y, rect.y + rect.height)
                );
            lumens.Add(t);

            lumenVelocities.Add(Random.insideUnitCircle);
        }
    }
    private void FixedUpdate()
    {
        if (!Application.isPlaying) return;
        for (int i = 0; i < lumens.Count; i++)
        {
            Transform t = lumens[i];
            Vector2 pos = t.position;
            Vector2 v = lumenVelocities[i];

            v += new Vector2(Random.Range(-1f, 1f), Random.Range(-1, 1f))*lumenAcceleration*Time.fixedDeltaTime;
            if (v.sqrMagnitude > lumenMaxSpeed*lumenMaxSpeed)
                v = lumenMaxSpeed * lumenMaxSpeed * v.normalized;
            if (v.y < 0f)
                v += Vector2.up * 0.01f;
            lumenVelocities[i] = v;

            pos += v;
            pos += lumenGravity * Time.fixedDeltaTime*Vector2.up;

            if (pos.y > rect.y + rect.height + lumenSizeConsideration ||
                pos.x < rect.x ||
                pos.x > rect.x + rect.width)
            {
                pos = new(Random.Range(rect.x, rect.x + rect.width), rect.y-lumenSizeConsideration);
            }
            t.position = pos;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(rect.center, rect.size);
    }
}
