using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string leadsTo = "MainMenu";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Ability.Instance.gameObject)
        {
            Dunkhelheit.Instance.FadeTo(leadsTo);
            enabled = false;
        }
    }
}
