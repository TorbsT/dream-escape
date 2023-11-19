using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog4 : DialogLevel
{
    private bool playerTrapped;
    private bool ballPressing;
    private bool pausing;
    private bool hasPaused;
    private bool HasResumed => !pausing && hasPaused;
    private bool lmao => playerTrapped && !ballPressing;
    // Start is called before the first frame update
    void Start()
    {
        DialogState initState = new(null);
        DialogState story1State = new("JIJO, listen up!");
        DialogState story2State = new("I know, I know, that door says \"exit,\" but it's all a trick.");
        DialogState story3State = new("Trust me, there are more levels, way beyond that door.");
        DialogState story4State = new("I've got surprises lined up for you. Don't fall for it!");
        DialogState story5State = new("Don't even think about escaping. Stay right here.");
        DialogState story6State = new("It's a wild dream party, and you're the guest of honor.");
        DialogState story7State = new("You don't want to miss out on the real fun, do you?");
        DialogState story8State = new("Ignore that door and just stay put. The real adventure is right here!");
        DialogState storyEndState = new(null, "storyEnd");

        initState.AddTransition(() => Life > 3f, story1State);
        story1State.AddTransition(() => Life > 10f, story2State);
        story2State.AddTransition(() => Life > 9f, story3State);
        story3State.AddTransition(() => Life > 8f, story4State);
        story4State.AddTransition(() => Life > 8f, story5State);
        story5State.AddTransition(() => Life > 7f, story6State);
        story6State.AddTransition(() => Life > 7f, story7State);
        story7State.AddTransition(() => Life > 6f, story8State);
        story8State.AddTransition(() => Life > 10f, storyEndState);

        DialogState failState = new("Oh, JIJO, you never fail to impress!", false);
        DialogState fail2State = new("Rolling that ball onto the pressure plate, entering the room like a true champion, and then, boom! The wall closes behind you.", false);
        DialogState fail3State = new("Pure genius! I must say, your strategic prowess is unmatched.", false);
        DialogState fail4State = new("And, oh, don't even think about clicking [R] to restart. It's not like this is the final level or anything. Definitely not.", false);
        DialogState fail5State = new("Keep struggling, my friend. There's plenty more \"not final\" levels ahead.", false);
        DialogState trueEndState = new(null, false);
        failState.AddTransition(() => Life > 5f, fail2State);
        fail2State.AddTransition(() => Life > 10f, fail3State);
        fail3State.AddTransition(() => Life > 5f, fail4State);
        fail4State.AddTransition(() => Life > 8f, fail5State);
        fail5State.AddTransition(() => Life > 8f, trueEndState);


        AddTransitionRange(() => lmao, failState,
            initState, story1State, story2State, story3State, story4State, story5State, story6State, story7State, story8State, storyEndState);

        DialogManager.Instance.SetStartDialogState(initState);
    }

    // Update is called once per frame
    void Update()
    {
        playerTrapped = GetTrigger("PlayerTrapped");
        ballPressing = GetTrigger("BallPressing");
        pausing = PauseMenu.Instance.Active;
        hasPaused = pausing || hasPaused;
    }

    private bool GetTrigger(string name) => DialogManager.Instance.HasTrigger(name);
}
