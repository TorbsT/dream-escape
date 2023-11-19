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

    private List<DialogState> history = new();
    private DialogState state;
    private DialogState startState;
    public void Trigger(string name) => triggers.Add(name);
    public void Untrigger(string name) => triggers.Remove(name);
    public bool HasTrigger(string name) => triggers.Contains(name);
    public void RememberState()
    {
        string currentId = state.recoveryId;
        string availableId = currentId;
        while (availableId == null)
        {
            if (history.Count == 0) break;
            availableId = history[^1].recoveryId;
            history.RemoveAt(history.Count-1);
        }
        if (availableId == null)
            Debug.Log("Could not remember the id of " + state.dialogMessage);
        else
        {
            Memory.Instance.RememberState(gameObject.scene.name, availableId);
        }
    }
    public void SetStartDialogState(DialogState state)
    {
        string previousId = Memory.Instance.GetState(gameObject.scene.name);
        if (previousId == null)
        {
            this.state = state;
            this.startState = state;
        } else
        {
            HashSet<int> explored = new();
            Queue<DialogState> statesToInvestigate = new();
            DialogState current = state;
            while (current.recoveryId != previousId)
            {
                Debug.Log(current.GetHashCode());
                explored.Add(current.GetHashCode());
                foreach (var next in current.GetTransitions())
                {
                    if (explored.Contains(next.NextState.GetHashCode()))
                        continue;
                    statesToInvestigate.Enqueue(next.NextState);
                }
                if (statesToInvestigate.Count == 0)
                {
                    current = null;
                    break;
                }
                current = statesToInvestigate.Dequeue();
            }
            if (current == null)
                Debug.LogWarning("Could not find recovery id " +previousId);
            this.state = current;
            this.startState = current;
        }
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
            if (state.EnterAction != null)
                state.EnterAction.Invoke();
            if (state.dialogMessage != null)
                dialog.text = Format(state.dialogMessage);
            else dialog.text = null;
            betweenDialogProgress = 0f;
        }

        float d = Time.unscaledDeltaTime/fadeDuration;
        if (active && state != null && state.dialogMessage != null) progress += d;
        else progress -= d;
        progress = Mathf.Clamp(progress, 0.0f, 1.0f);

        if (progress >= 1f || (state != null && state.dialogMessage == null))
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
                history.Add(state);
                state = newState;
            }
        }
    }
    private string Format(string input)
    {
        string result = input.Replace("<i>", "<i angle=20>");
        result = result.Replace("{good}", Memory.Instance.Treasury.GetRandom());
        return result;
    }
}
public class DialogState
{
    public string recoveryId;
    public string dialogMessage;
    public Action EnterAction;
    private List<DialogTransition> transitions = new();

    public DialogState(string message, bool remember = true)
    {
        dialogMessage = message;
        if (remember)
            recoveryId = message;
    }
    public DialogState(string message, string recoveryId)
    {
        dialogMessage = message;
        this.recoveryId = recoveryId;
    }
    public List<DialogTransition> GetTransitions() => transitions;
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