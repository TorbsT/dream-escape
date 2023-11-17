using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog1 : MonoBehaviour
{
    private bool hasMoved;
    private bool hasJumped;
    private bool hasUsedPower;
    private bool hasExpired;
    private bool hasTriedTooMuch;

    // Start is called before the first frame update
    void Start()
    {
        DialogState initialState = new("Press [A] or [D] to move.");
        DialogState pressJumpState = new("Press [Space] to jump.");
        DialogState pressEState = new("Press [E] to lucid dream.");
        DialogState reachDoorState = new("Reach the door to escape this room.");
        DialogState expiredState = new(null);
        DialogState failedPowerUse = new("You can only use Lucid 2 times. Press [R] to restart.");



        initialState.AddTransition(() => hasMoved, pressJumpState);
        pressJumpState.AddTransition(() => hasJumped, pressEState);
        pressEState.AddTransition(() => hasUsedPower, reachDoorState);
        reachDoorState.AddTransition(() => hasExpired, expiredState);

        DialogTransition failedPowerT = new(() => hasTriedTooMuch, failedPowerUse);
        initialState.AddTransition(failedPowerT);
        pressJumpState.AddTransition(failedPowerT);
        pressEState.AddTransition(failedPowerT);
        reachDoorState.AddTransition(failedPowerT);
        expiredState.AddTransition(failedPowerT);

        DialogManager.Instance.SetStartDialogState(initialState);
    }
    private void Update()
    {
        hasMoved = hasMoved || Input.GetAxisRaw("Horizontal") != 0;
        hasJumped = hasJumped || (Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Vertical") > 0f);
        hasUsedPower = hasUsedPower || Input.GetKeyDown(KeyCode.E);
        hasExpired = DialogManager.Instance.DialogLife > 5f;
        hasTriedTooMuch = Ability.Instance.HasFailedUse;
    }
}
