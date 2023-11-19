using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogLevel : MonoBehaviour
{
    protected float Life => DialogManager.Instance.DialogLife;
    protected bool DontRestart => M.GetBool("dontrestart1");
    protected bool HasUsedPower => TimesUsedPower > 0;
    protected int TimesUsedPower => Ability.Instance.TimesUsed;
    protected float Friend { get => M.Friend; set => M.Friend = value; }
    protected int RebelRestarts { get => M.RebelRestarts; set => M.RebelRestarts = value; }
    protected Memory M => Memory.Instance;
    protected bool IsTrigger(string name) => DialogManager.Instance.HasTrigger(name);
    protected void AddTransitionRange(Func<bool> condition, DialogState next, params DialogState[] states)
        => AddTransitionRange(new(condition, next), states);
    protected void AddTransitionRange(DialogTransition transition, params DialogState[] states)
    {
        foreach (var state in states)
        {
            state.AddTransition(transition);
        }
    }
}
