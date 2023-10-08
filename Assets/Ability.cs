using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    public static Ability Instance { get; private set; }
    public event Action<bool> Used;
    [field: SerializeField] public bool RED { get; private set; }
    [field: SerializeField] public Color REDColor { get; private set; } = Color.red;
    [field: SerializeField] public Color BLUColor { get; private set; } = Color.cyan;

    private void Awake()
    {
        Instance = this;
        RED = false;
    }
    private void Start()
    {
        UseAbility();
    }
    public void OnUseAbility(InputAction.CallbackContext context)
    {
        UseAbility();
    }
    public void UseAbility()
    {
        RED = !RED;
        foreach (var comp in FindObjectsByType<DimensionComponent>(FindObjectsSortMode.None))
        {
            comp.SetPhysical(RED == comp.RED);
        }

        Camera.main.backgroundColor = RED ? BLUColor : REDColor;
        gameObject.layer = RED ? 8 : 9;
        Used?.Invoke(RED);
    }
}