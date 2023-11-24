using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    public static Ability Instance { get; private set; }
    public event Action<bool, bool> Used;
    public event Action FailedUse;
    public bool HasFailedUse { get; private set; }
    [field: SerializeField] public bool RED { get; private set; }
    [field: SerializeField] public Color REDColor { get; private set; } = Color.red;
    [field: SerializeField] public Color BLUColor { get; private set; } = Color.cyan;

    public int TimesUsed { get; private set; } = 0;
    private float timeSinceAbility;
    private List<AudioSource> audios = new();

    private void Awake()
    {
        Instance = this;
        RED = false;
        foreach (var src in GetComponents<AudioSource>())
            audios.Add(src);
    }
    private void Start()
    {
        UseAbility(true);
    }
    public void OnUseAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseAbility();
        }
    }
    public void UseAbility(bool initial = false)
    {
        if (TimesUsed >= 2)
        {
            AudioManager.Instance.Play($"faillucid", 0.45f);
            FailedUse?.Invoke();
            HasFailedUse = true;
            return;
        }

        RED = !RED;
        timeSinceAbility = initial ? 100f : 0f;
        foreach (var comp in FindObjectsByType<DimensionComponent>(FindObjectsSortMode.None))
        {
            bool show =
                (comp.Dimension == Dimension.PURP) ||
                (comp.Dimension == Dimension.RED && RED) ||
                (comp.Dimension == Dimension.BLU && !RED);
            comp.SetPhysical(show);
            if (comp.Dimension == Dimension.PURP)
            {
                bool isPlayer = comp.gameObject == gameObject;
                if (!isPlayer)
                    comp.gameObject.layer = (RED ? 10 : 11);
            }
        }

        if (!initial)
        {
            ParticleManager.Instance.Spawn("lucid", Head.Instance.transform.position);
            TimeStop.Instance.Stop(0f);
            AudioManager.Instance.Play($"lucid{TimesUsed % 2}", 0.6f);
            TimesUsed++;
        }

        MusicSystem.Instance.UsedAbility(RED, initial);
        Used?.Invoke(RED, initial);
    }
    private void Update()
    {
        timeSinceAbility += Time.deltaTime;
        bool timePassed = timeSinceAbility >= LucidMask.Instance.DispandAnimDuration;
        bool showUpdatedRED = RED && timePassed;
        gameObject.layer = (showUpdatedRED ? 8 : 9);
    }
}
