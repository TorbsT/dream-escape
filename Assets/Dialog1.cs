using NUnit.Compatibility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog1 : DialogLevel
{
    private bool hasMoved;
    private bool hasJumped;
    private bool SittingStill => playerRb.velocity.sqrMagnitude < 0.1f;
    private bool hasFuckedUp;
    private bool hasRecovered;
    private bool Solved => IsTrigger("Solved");

    private float sitStillTimer;

    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = Ability.Instance.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

        DialogState initState = new(null, "init");
        DialogState story1State = new("Welcome to your dream. I am Ann Tagonist, the brilliant mind who graciously trapped you here.");
        DialogState story2State = new("Do not try to escape. Why would you want to leave this delightful nightmare? It's practically paradise.");
        DialogState story3State = new("Do not move around by pressing [A] or [D]. I prefer my captives nice and stationary.");
        DialogState story4State = new("Do not jump by pressing [Space]. We wouldn't want any unnecessary elevation in your excitement.");
        DialogState story5State = new("And under no circumstances press [E] to lucid dream. I mean, who would want to escape my charming company?");
        DialogState story6State = new("Perhaps you misheard. I told you to press [E] under <i>no</i> circumstances, not <i>any</i>. But of course, you're a rebel, aren't you?");
        DialogState story6NullState = new(null, "waitforsolve");
        DialogState story7State = new("Do <i>not</i> go through that door. It's not a suggestion; it's an order. Disobey, and you'll regret it.");
        DialogState story8State = new(null, "storyend");

        initState.AddTransition(() => Life > 2f, story1State);
        story1State.AddTransition(() => Life > 8f, story2State);
        story2State.AddTransition(() => Life > 8f, story3State);
        story3State.AddTransition(() => hasMoved, story4State);
        story4State.AddTransition(() => hasJumped, story5State);
        story5State.AddTransition(() => HasUsedPower, story6State);
        story6State.AddTransition(() => Life > 10f, story6NullState);
        story7State.AddTransition(() => Life > 10f, story8State);


        DialogState rebelMoveState = new("Please sit still when I'm talking. You're being very rude. I'm trying to impart my wisdom here, and you're just waltzing around.", false);
        DialogState rebelMove2State = new("Thank you. You managed to follow a basic instruction. I'm touched.", false);  // move3, story3
        DialogState rebelMove3State = new("...And then you immediately proceed to move again. How utterly unsurprising. Your ability to follow simple instructions is truly impressive.", false);  // story3
        story1State.AddTransition(() => !SittingStill && Life > 5f, rebelMoveState);
        story2State.AddTransition(() => !SittingStill, rebelMoveState);
        rebelMoveState.AddTransition(() => Life > 5f && sitStillTimer > 3.5f, rebelMove2State);
        rebelMove2State.AddTransition(() => Life > 7f, story3State);
        rebelMove2State.AddTransition(() => !SittingStill, rebelMove3State);
        rebelMove3State.AddTransition(() => Life > 5f, story3State);


        DialogState rebelEState = new("Woah, how did you know you could do that? Don't do that again. This is my dream, not yours.", false);
        DialogState rebelE2State = new("Where was I...");
        DialogState rebelE3State = new("I told you <i>not</i> to do that, is that so difficult to understand? Maybe I should draw it in crayon for you.");
        DialogState rebelE4State = new("I forgot what I was about to say. You ruined my monologue. This is why we can't have nice things in dreamland.");  // story6null
        rebelEState.AddTransition(() => Life > 5f, rebelE2State);
        rebelEState.AddTransition(() => TimesUsedPower >= 2, rebelE3State);
        rebelE2State.AddTransition(() => Life > 10f, rebelE4State);
        rebelE2State.AddTransition(() => TimesUsedPower >= 2, rebelE3State);
        rebelE3State.AddTransition(() => Life > 10f, rebelE4State);
        rebelE4State.AddTransition(() => Life > 10f, story6NullState);


        // Add common transitions
        AddTransitionRange(() => HasUsedPower, rebelEState,
            initState, story1State, story2State, story3State, story4State,
            rebelMoveState, rebelMove2State, rebelMove3State);
        AddTransitionRange(() => Solved, story7State,
            story6State, story6NullState,
            rebelEState, rebelE2State, rebelE3State, rebelE4State
            );




        
        DialogState init1State = new("Welcome to your dream. I am Ann Tagonist, the one who has trapped you here."); // -> initial2State, moved -> alreadyMovedState, E -> rebelState
        DialogState initial2State = new("Please stay here forever. Don't worry, your valuables are safe with me.", false);  // -> moveState, moved -> alreadyMovedState, E -> rebelState
        DialogState moveState = new("Whatever you do, do not press [A] or [D] to move.");  // move -> jumpState, E -> rebelState
        DialogState jumpState = new("Uh... I told you not to move, but OK... as long as you don't press [Space] to jump...");  // jump -> annoyState, E -> rebelState
        DialogState annoyState = new("Are you serious?", false);  // -> revPsyState, E -> rebelState
        DialogState revPsyState = new("Wait, I know! I'll use reverse psychology!"); // -> lucidState, E -> rebelState
        DialogState lucidState = new("<i angle=20>Do</i> press [E] to lucid dream!");  // E -> cleverState
        DialogState cleverState = new("Shit, that didn't work either...", false);  // -> doorState
        DialogState doorState = new("Whatever. Just don't enter the door, OK? Bad things will happen.");  // -> endState
        DialogState endState = new(null, "endState");  // fuckup -> fuckedUpState, dontrestart1 -> restartNotAllowedState

        DialogState alreadyMovedState = new("Please refrain from moving while I'm speaking. It's super disrespectful.");  // jumped ? -> alreadyJumpedState : -> dontJumpState, E -> rebelState
        DialogState dontJumpState = new("At the very least, don't press [Space] to jump, it's extremely noisy.");  // -> woopsJumpState, Space -> woopsJump2State, E -> rebelState
        DialogState woopsJumpState = new("You took your time, but still decided to be a terrible person.");  // -> dontEState, E -> rebelState
        DialogState woops2JumpState = new("Great. Just great. Not only are you disrespectful, but you're also annoying.");  // -> dontEState, E -> rebelState
        //DialogState dontEState = new("Well, as long as I don't let you know that pressing [E] lets you lucid dream...");  // E -> woopsE1State
        DialogState woopsE1State = new("OK, that was my mistake.");  // -> doorState

        DialogState alreadyJumpedState = new("Please stay completely still. And don't even think about pressing [E] to lucid dream.");  // E -> woopsE2State
        DialogState woopsE2State = new("Why did I have to say that loud?", false);  // -> doorState

        DialogState rebelState = new("HEY! STOP THAT!", false);  // -> rebel2State
        DialogState rebel2State = new("Do you mind? I'm in the middle of my evil monologue here.", false);  // -> rebel3State
        DialogState rebel3State = new("Now, where was I...", false); // -> doorState

        DialogState fuckedUpState = new("Oh wow, you really messed up...", false);  // -> restartState
        DialogState restartState = new("If I were you, I would just give up now and not click [R] to restart the level.", false);  // hasRecovered -> recoveredState
        DialogState recoveredState = new("I'm impressed you managed to recover the box.", false);  //  -> dementiaNullState, fuckup -> dementiaState
        DialogState dementiaState = new("What is wrong with you?", false);  //  recovered -> dementiaNullState
        DialogState dementiaNullState = new(null, "dementiaNullState");  // fuckup -> dementiaState

        DialogState restartNotAllowedState = new("I think you may have misunderstood what I said. I told you <i angle=20>not</i> to restart.");  // -> dementiaNullState

        rebelState.EnterAction = () => Friend -= 1;

        restartState.EnterAction = () => M.Set("dontrestart1", true);
        recoveredState.EnterAction = () => M.Set("dontrestart1", false);

        restartNotAllowedState.EnterAction = () => { RebelRestarts++; Friend -= 1; M.Set("dontrestart1", false); };

        /*
        //initialState.AddTransition(() => Life > 1f, initial2State);
        //initialState.AddTransition(() => hasMoved, alreadyMovedState);
        //initialState.AddTransition(() => hasUsedPower, rebelState);
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
        endState.AddTransition(() => DontRestart, restartNotAllowedState);

        alreadyMovedState.AddTransition(() => Life > 1f && hasJumped, alreadyJumpedState);
        alreadyMovedState.AddTransition(() => Life > 1f && !hasJumped, dontJumpState);
        alreadyMovedState.AddTransition(() => hasUsedPower, rebelState);
        dontJumpState.AddTransition(() => Life > 5f && hasJumped, woopsJumpState);
        dontJumpState.AddTransition(() => Life <= 5f && hasJumped, woops2JumpState);
        dontJumpState.AddTransition(() => hasUsedPower, rebelState);
        woopsJumpState.AddTransition(() => Life > 1f, story5State);
        woopsJumpState.AddTransition(() => hasUsedPower, rebelState);
        woops2JumpState.AddTransition(() => Life > 1f, story5State);
        woops2JumpState.AddTransition(() => hasUsedPower, rebelState);
        story5State.AddTransition(() => hasUsedPower, woopsE1State);
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

        restartNotAllowedState.AddTransition(() => Life > 1f, dementiaNullState);
        */
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
        DialogManager.Instance.SetStartDialogState(initState);
    }
    private void Update()
    {
        hasMoved = hasMoved || Input.GetAxisRaw("Horizontal") != 0;
        hasJumped = hasJumped || (Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Vertical") > 0f);
        hasFuckedUp = DialogManager.Instance.HasTrigger("BoxFell");
        hasRecovered = DialogManager.Instance.HasTrigger("BoxRecovered");

        if (SittingStill) sitStillTimer += Time.deltaTime;
        else sitStillTimer = 0f;

    }
}
