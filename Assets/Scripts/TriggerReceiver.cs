using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerReceiver : MonoBehaviour
{
    public abstract void Trigger(GameObject source, bool power);
}
