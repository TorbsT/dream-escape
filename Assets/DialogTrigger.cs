using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogTrigger : MonoBehaviour
{
    private new BoxCollider2D collider;
    [SerializeField] private string triggerTag = "Player";
    private int count = 0;
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != triggerTag) return;

        count++;
        if (count == 1)
            DialogManager.Instance.Trigger(gameObject.name);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != triggerTag) return;

        count--;
        if (count == 0)
            DialogManager.Instance.Untrigger(gameObject.name);
    }
}
