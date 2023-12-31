using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Key : MonoBehaviour
{
    [field: SerializeField] public List<TriggerReceiver> Receivers { get; private set; } = new();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var item in Receivers)
        {
            if (item == null) continue;
            Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var item in Receivers)
            item.Trigger(gameObject, true);
        //Invoke(nameof(Reactivate), 1.5f);
        TimeStop.Instance.Stop(0f);
        CameraManager.Instance.Shake(CameraManager.Instance.LeverPreset);
        AudioManager.Instance.Play("lever");
        gameObject.SetActive(false);
    }
    private void Reactivate()
    {
        gameObject.SetActive(true);
    }
}
