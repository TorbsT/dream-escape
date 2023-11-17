using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog1 : MonoBehaviour
{
    private bool hasMoved;
    private bool hasJumped;
    private bool hasUsedPower;
    private bool hasFuckedUp;
    private bool hasRecovered;
    private float Life => DialogManager.Instance.DialogLife;

    // Start is called before the first frame update
    void Start()
    {
        DialogState initialState = new("Welcome to your dream. I am Ann Tagonist, the one who has trapped you here."); // -> initial2State, moved -> alreadyMovedState, E -> rebelState
        DialogState initial2State = new("Please stay here forever. Don't worry, your valuables are safe with me.");  // -> moveState, moved -> alreadyMovedState, E -> rebelState
        DialogState moveState = new("Whatever you do, do not press [A] or [D] to move.");  // move -> jumpState, E -> rebelState
        DialogState jumpState = new("Uh... I told you not to move, but OK... as long as you don't press [Space] to jump...");  // jump -> annoyState, E -> rebelState
        DialogState annoyState = new("Are you serious?");  // -> revPsyState, E -> rebelState
        DialogState revPsyState = new("Wait, I know! I'll use reverse psychology!"); // -> lucidState, E -> rebelState
        DialogState lucidState = new("<i angle=20>Do</i> press [E] to lucid dream!");  // E -> cleverState
        DialogState cleverState = new("Shit, that didn't work either...");  // -> doorState
        DialogState doorState = new("Whatever. Just don't enter the door, OK? Bad things will happen.");  // -> endState
        DialogState endState = new(null);  // fuckup -> fuckedUpState

        DialogState alreadyMovedState = new("Please refrain from moving while I'm speaking. It's super disrespectful.");  // jumped ? -> alreadyJumpedState : -> dontJumpState, E -> rebelState
        DialogState dontJumpState = new("At the very least, don't press [Space] to jump, it's extremely noisy.");  // -> woopsJumpState, Space -> woopsJump2State, E -> rebelState
        DialogState woopsJumpState = new("You took your time, but still decided to be a terrible person.");  // -> dontEState, E -> rebelState
        DialogState woops2JumpState = new("Great. Just great. Not only are you disrespectful, but you're also annoying.");  // -> dontEState, E -> rebelState
        DialogState dontEState = new("Well, as long as I don't let you know that pressing [E] lets you lucid dream...");  // E -> woopsE1State
        DialogState woopsE1State = new("OK, that was my mistake.");  // -> doorState

        DialogState alreadyJumpedState = new("Please stay completely still. And don't even think about pressing [E] to lucid dream.");  // E -> woopsE2State
        DialogState woopsE2State = new("Why did I have to say that loud?");  // -> doorState

        DialogState rebelState = new("HEY! STOP THAT!");  // -> rebel2State
        DialogState rebel2State = new("Do you mind? I'm doing my evil monologue here.");  // -> rebel3State
        DialogState rebel3State = new("Now, where was I..."); // -> doorState

        DialogState fuckedUpState = new("Oh wow, you really messed up...");  // -> restartState
        DialogState restartState = new("If I were you, I would just give up now and not click [R] to restart the level.");  // hasRecovered -> recoveredState
        DialogState recoveredState = new("I'm impressed you managed to recover the box.");  //  -> dementiaNullState, fuckup -> dementiaState
        DialogState dementiaState = new("What is wrong with you?");  //  recovered -> dementiaNullState
        DialogState dementiaNullState = new(null);  // fuckup -> dementiaState

        initialState.AddTransition(() => Life > 1f, initial2State);
        initialState.AddTransition(() => hasMoved, alreadyMovedState);
        initialState.AddTransition(() => hasUsedPower, rebelState);
        initial2State.AddTransition(() => Life > 1f, moveState);
        initial2State.AddTransition(() => hasMoved, alreadyMovedState);
        initial2State.AddTransition(() => hasUsedPower, rebelState);
        moveState.AddTransition(() => hasMoved, jumpState);
        moveState.AddTransition(() => hasUsedPower, rebelState);
        jumpState.AddTransition(() => hasJumped, annoyState);
        jumpState.AddTransition(() => hasUsedPower, rebelState);
        annoyState.AddTransition(() => Life > 1f, revPsyState);
        annoyState.AddTransition(() => hasUsedPower, rebelState);
        revPsyState.AddTransition(() => Life > 1f, lucidState);
        revPsyState.AddTransition(() => hasUsedPower, rebelState);
        lucidState.AddTransition(() => hasUsedPower, cleverState);
        cleverState.AddTransition(() => Life > 1f, doorState);
        doorState.AddTransition(() => Life > 1f, endState);
        endState.AddTransition(() => hasFuckedUp, fuckedUpState);

        alreadyMovedState.AddTransition(() => Life > 1f && hasJumped, alreadyJumpedState);
        alreadyMovedState.AddTransition(() => Life > 1f && !hasJumped, dontJumpState);
        alreadyMovedState.AddTransition(() => hasUsedPower, rebelState);
        dontJumpState.AddTransition(() => Life > 5f && hasJumped, woopsJumpState);
        dontJumpState.AddTransition(() => Life <= 5f && hasJumped, woops2JumpState);
        dontJumpState.AddTransition(() => hasUsedPower, rebelState);
        woopsJumpState.AddTransition(() => Life > 1f, dontEState);
        woopsJumpState.AddTransition(() => hasUsedPower, rebelState);
        woops2JumpState.AddTransition(() => Life > 1f, dontEState);
        woops2JumpState.AddTransition(() => hasUsedPower, rebelState);
        dontEState.AddTransition(() => hasUsedPower, woopsE1State);
        woopsE1State.AddTransition(() => Life > 1f, doorState);

        alreadyJumpedState.AddTransition(() => hasUsedPower, woopsE2State);
        woopsE2State.AddTransition(() => Life > 1f, doorState);

        rebelState.AddTransition(() => Life > 1f, rebel2State);
        rebel2State.AddTransition(() => Life > 1f, rebel3State);
        rebel3State.AddTransition(() => Life > 1f, doorState);

        fuckedUpState.AddTransition(() => Life > 1f, restartState);
        restartState.AddTransition(() => hasRecovered, recoveredState);
        recoveredState.AddTransition(() => Life > 1f, dementiaNullState);
        recoveredState.AddTransition(() => hasFuckedUp, dementiaState);
        dementiaState.AddTransition(() => hasRecovered, dementiaNullState);
        dementiaNullState.AddTransition(() => hasFuckedUp, dementiaState);


        /*
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
        */
        DialogManager.Instance.SetStartDialogState(initialState);
    }
    private void Update()
    {
        hasMoved = hasMoved || Input.GetAxisRaw("Horizontal") != 0;
        hasJumped = hasJumped || (Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Vertical") > 0f);
        hasUsedPower = hasUsedPower || Input.GetKeyDown(KeyCode.E);
        hasFuckedUp = DialogManager.Instance.HasTrigger("BoxFell");
        hasRecovered = DialogManager.Instance.HasTrigger("BoxRecovered");
    }
}
