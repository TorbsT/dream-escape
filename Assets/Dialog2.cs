using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Dialog2 : DialogLevel
{
    private bool playerTrapped;
    private bool boxTrapped;
    private bool pushingBox;
    private bool playerTrying;
    private bool solved;
    private bool fun;

    // Start is called before the first frame update
    void Start()
    {
        string mostValuable = M.GetString("mostValuable");
        if (mostValuable == null)
        {
            mostValuable = Memory.Instance.Treasury.GetRandom();
            M.Set("mostValuable", mostValuable);
        }


        DialogState initState = new(null);
        DialogState story1 = new("Are you deaf? I explicitly told you not to go through that door.");
        DialogState story2 = new("Now, you've really ticked me off.");
        DialogState story3 = new("You see, I have a motive, a simple one. I'm going to take everything from your home, and I mean everything.");
        DialogState story4 = new("Your furniture, your socks, even your most prized possession.");
        DialogState story5 = new("That's right, I'll even take your most valuable trinket:");
        DialogState story6 = new($"{mostValuable}.");
        DialogState story7 = new("Brace yourself, JIJO, you're in for a rocky ride!");
        DialogState story7Null = new(null, "stor7");
        DialogState story8 = new("Well, well, JIJO! Color me impressed; you actually managed to stumble your way through.");
        DialogState story9 = new("But hold on a minute, before you pat yourself on the back, let's talk about this. Going through that door might not be in your best interest.");
        DialogState story10 = new("Trust me, it's not all sunshine and rainbows on the other side. Think twice before you take that step, my friend.");
        DialogState storyEnd = new(null, "end");

        initState.AddTransition(() => Life > 1f, story1);
        story1.AddTransition(() => Life > 8f, story2);
        story2.AddTransition(() => Life > 8f, story3);
        story3.AddTransition(() => Life > 8f, story4);
        story4.AddTransition(() => Life > 8f, story5);
        story5.AddTransition(() => Life > 8f, story6);
        story6.AddTransition(() => Life > 8f, story7);
        story7.AddTransition(() => Life > 8f, story7Null);
        
        story8.AddTransition(() => Life > 8f, story9);
        story9.AddTransition(() => Life > 8f, story10);
        story10.AddTransition(() => Life > 8f, storyEnd);

        story6.EnterAction = () => M.Set("seenMostValuable", true);



        DialogState fuckState = new("Oh, poor JIJO! You pushed that box, opened the hatch, and now you're stuck with no way up.", false);
        DialogState fuck2State = new("Isn't that just the epitome of bad luck?", false);
        DialogState fuck3State = new("You can't reach the box, and guess what? Clicking [R] won't change a thing.", false);
        DialogState fuck4State = new("Instead of trying again, why not just admit defeat?", false);
        DialogState fuck5State = new("Give up, JIJO. This dream world is proving to be a tough nut to crack, and the road ahead isn't looking any easier.", false);
        DialogState fuck6State = new("Embrace the sweet taste of failure; it suits you!", false);
        DialogState trueEndState = new(null, false);

        fuckState.AddTransition(() => Life > 8f, fuck2State);
        fuck2State.AddTransition(() => Life > 5f, fuck3State);
        fuck3State.AddTransition(() => Life > 8f, fuck4State);
        fuck4State.AddTransition(() => Life > 8f, fuck5State);
        fuck5State.AddTransition(() => Life > 8f, fuck6State);
        fuck6State.AddTransition(() => Life > 8f, trueEndState);


        AddTransitionRange(() => playerTrying && boxTrapped, fuckState,
            initState, story1, story2, story3, story4, story5, story6, story7, story7Null);
        AddTransitionRange(() => solved, story8,
            initState, story1, story2, story3, story4, story5, story6, story7, story7Null);

        DialogState initialState = new("I thought I told you not to enter the door?");  // -> impossibruState
        DialogState impossibruState = new("Well, it doesn't matter. It's impossible to reach the next door anyways.");  // -> silentState
        DialogState silentState = new(null, "silentState");  // playerTrying & boxTrapped -> failedState, pushingBox -> noState
        DialogState failedState = new("See? It's impossible. Just give up. Do not press [R].", false);  // -> failedNullState
        DialogState failedNullState = new(null, "failedNullState");
        DialogState noState = new("Nonononono stop that. There's no way out of here.", false);  // solved -> solvedState
        DialogState solvedState = new(null, "solvedState");

        DialogState rebelRestart1State = new("I think you may have misunderstood what I said. I told you <i angle=20>not</i> to restart.");

        DialogState rebelRestart2State = new("You just restarted again! Directly disobeying orders! Unbelievable.");




        initialState.AddTransition(() => Life > 1f, impossibruState);
        impossibruState.AddTransition(() => Life > 1f, silentState);
        silentState.AddTransition(() => playerTrying && boxTrapped, failedState);
        silentState.AddTransition(() => pushingBox, noState);
        failedState.AddTransition(() => Life > 1f, failedNullState);
        noState.AddTransition(() => solved, solvedState);

        DialogManager.Instance.SetStartDialogState(initState);
    }

    // Update is called once per frame
    void Update()
    {
        boxTrapped = GetTrigger("BoxTrapped");
        pushingBox = GetTrigger("PushingBox");
        playerTrying = GetTrigger("PlayerTrying");
        fun = GetTrigger("Fun");
        solved = GetTrigger("Solved");
    }
    private bool GetTrigger(string name) => DialogManager.Instance.HasTrigger(name);
}
