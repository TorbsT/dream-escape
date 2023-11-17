using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [field: SerializeField] public float DialogLife { get; private set; }

    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI dialog;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float betweenDialogDuration = 1f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public static DialogManager Instance { get; private set; }
    private float progress;
    private float betweenDialogProgress;
    private bool active;
    private HashSet<string> triggers = new();

    private DialogState state;
    public void Trigger(string name) => triggers.Add(name);
    public void Untrigger(string name) => triggers.Remove(name);
    public bool HasTrigger(string name) => triggers.Contains(name);
    public void SetStartDialogState(DialogState state)
    {
        this.state = state;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        float db = Time.unscaledDeltaTime / betweenDialogDuration;
        if (progress <= 0f) betweenDialogProgress += db;
        if (betweenDialogProgress >= 1f && state != null)
        {
            active = true;
            dialog.text = state.dialogMessage;
            betweenDialogProgress = 0f;
        }

        float d = Time.unscaledDeltaTime/fadeDuration;
        if (active && state != null && state.dialogMessage != null) progress += d;
        else progress -= d;
        progress = Mathf.Clamp(progress, 0.0f, 1.0f);

        if (progress >= 1f)
            DialogLife += Time.unscaledDeltaTime;
        else
            DialogLife = 0f;

        float curveProgress = fadeCurve.Evaluate(progress);
        dialog.color = new(1f, 1f, 1f, curveProgress);
        bg.color = new(0f, 0f, 0f, curveProgress);

        if (state != null)
        {
            DialogState newState = state.Check();
            if (newState != null && (progress == 0f || progress == 1f))
            {
                active = false;
                state = newState;
            }
        }
    }
}
public class DialogState
{
    public string dialogMessage;
    private List<DialogTransition> transitions = new();

    public DialogState(string message)
    {
        dialogMessage = message;
    }
    public void AddTransition(Func<bool> condition, DialogState next)
        => AddTransition(new DialogTransition(condition, next));
    public void AddTransition(DialogTransition transition)
        => transitions.Add(transition);
    public DialogState Check()
    {
        foreach (var transition in transitions)
            if (transition.Condition())
                return transition.NextState;
        return null;
    }
}
public class DialogTransition
{
    public Func<bool> Condition { get; private set; }
    public DialogState NextState { get; private set; }

    public DialogTransition(Func<bool> condition, DialogState nextState)
    {
        Condition = condition;
        NextState = nextState;
    }
}