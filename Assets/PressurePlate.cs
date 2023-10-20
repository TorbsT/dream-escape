using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PressurePlate : MonoBehaviour
{
    [field: SerializeField] public List<TriggerReceiver> Receivers { get; private set; }
    [field: SerializeField] public bool Active { get; private set; }
    private int colls = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (var r in Receivers)
        {
            if (r == null) continue;
            Gizmos.DrawLine(transform.position, r.transform.position);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        colls++;
        RefreshCharge();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colls--;
        RefreshCharge();
    }
    private void RefreshCharge()
    {
        if (!Application.isPlaying) return;
        bool newActive = colls > 0;
        if (Active == newActive) return;
        foreach (var r in Receivers)
        {
            r.Trigger(newActive);
        }
        Active = newActive;
    }
}
