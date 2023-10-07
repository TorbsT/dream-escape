using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TriggerReceiver
{
    public override void Trigger()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
