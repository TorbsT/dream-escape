using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog3 : DialogLevel
{
    [SerializeField] private GameObject leftKeyGO;
    [SerializeField] private GameObject rightKeyGO;


    private bool leftKey => !leftKeyGO.activeSelf;
    private bool rightKey => !rightKeyGO.activeSelf;
    private bool playerTrapped => GetTrigger("PlayerTrapped");
    private bool playerDown => GetTrigger("PlayerDown");

    private float timeTrapped;
    
    // Start is called before the first frame update
    void Start()
    {
        string mostValuable = M.GetString("mostValuable");
        bool seenFav = M.GetBool("seenMostValuable");

        DialogState initState = new(null, "init");
        DialogState story1State = new("JIJO, JIJO, JIJO. Let's make a deal.");
        DialogState story2State = new($"I promise not to take your most prized possession.");
        DialogState story22State = new($"That's right: {mostValuable}.", false);
        DialogState story3State = new($"If you just stop trying to escape, I promise not to take {mostValuable}.");
        DialogState story4State = new($"Isn't that a generous offer? You can keep {mostValuable} safe and sound.");
        DialogState story5State = new($"My only condition is: just stay put and enjoy the dream.");
        DialogState story6State = new($"Trust me; it's for your own good.");
        DialogState storyEndState = new(null, "storyEnd");

        initState.AddTransition(() => Life > 1f, story1State);
        story1State.AddTransition(() => !seenFav && Life > 5f, story2State);
        story1State.AddTransition(() => seenFav && Life > 5f, story3State);
        story2State.AddTransition(() => Life > 8f, story22State);
        story22State.AddTransition(() => Life > 8f, story4State);
        story3State.AddTransition(() => Life > 8f, story4State);
        story4State.AddTransition(() => Life > 8f, story5State);
        story5State.AddTransition(() => Life > 8f, story6State);
        story6State.AddTransition(() => Life > 8f, storyEndState);

        DialogState fellState = new("Oh, fantastic move, JIJO! You're down there, and guess what? There's no way back up.", false);
        DialogState fell2State = new("Thinking about hitting that [R] key? Forget it! Just admit defeat.", false);
        DialogState fell3State = new("This dream's turning into a nightmare for you, and trust me, it's only going to get worse.", false);
        DialogState fell4State = new("Save yourself the trouble and accept that your dream is my playground.", false);
        DialogState fell4NullState = new(null, false);
        fellState.AddTransition(() => Life > 10f, fell2State);
        fell2State.AddTransition(() => Life > 10f, fell3State);
        fell3State.AddTransition(() => Life > 10f, fell4State);
        fell4State.AddTransition(() => Life > 10f, fell4NullState);

        DialogState gotchaState = new("Oh, look at that, JIJO! You managed to lock yourself in. Brilliant move!", false);
        DialogState gotcha2State = new("Now, what's your grand plan? Click [R] and try again?", false);
        DialogState gotcha3State = new("How about you save us both the trouble and just give up? It's not getting any better for you.", false);
        DialogState trueEndState = new(null, false);
        gotchaState.AddTransition(() => Life > 10f, gotcha2State);
        gotcha2State.AddTransition(() => Life > 10f, gotcha3State);
        gotcha3State.AddTransition(() => Life > 10f, trueEndState);

        DialogState itoldyouState = new("Bravo, JIJO, bravo! Look at you, the master escape artist, finding a way back up. A standing ovation is in order!", false);
        DialogState itoldyou2State = new("Oh, wait, what's this? You've locked yourself in a room now? Absolutely brilliant.", false);
        DialogState itoldyou3State = new("Clearly, I underestimated your problem-solving skills.", false);
        DialogState itoldyou4State = new("And just in case you're considering clicking [R] to restart, well, go ahead – maybe it'll lead to an even more impressive outcome.", false);
        DialogState itoldyou5State = new("Enjoy your little room, JIJO. It's practically a penthouse, isn't it?", false);
        itoldyouState.AddTransition(() => Life > 10f, itoldyou2State);
        itoldyou2State.AddTransition(() => Life > 10f, itoldyou3State);
        itoldyou3State.AddTransition(() => Life > 10f, itoldyou4State);
        itoldyou4State.AddTransition(() => Life > 10f, itoldyou5State);
        itoldyou5State.AddTransition(() => Life > 10f, trueEndState);
        

        AddTransitionRange(() => timeTrapped > 2f, gotchaState,
            initState, story1State, story2State, story22State, story3State, story4State, story5State, story6State, storyEndState);
        AddTransitionRange(() => timeTrapped > 2f, itoldyouState,
            fellState, fell2State, fell3State, fell4State, fell4NullState);
        AddTransitionRange(() => leftKey && !rightKey && playerDown, fellState,
            initState, story1State, story2State, story22State, story3State, story4State, story5State, story6State, storyEndState);

        /*
        DialogState initialState = new(null, "yeppsh");  // !leftKey & rightKey -> gotchaState, leftKey & !rightKey & playerDown -> closeState
        DialogState gotchaState = new("Gotcha", false);
        DialogState closeState = new("That was close", false);

        initialState.AddTransition(() => rightKey && TimesUsedPower % 2 == 0, gotchaState);
        initialState.AddTransition(() => leftKey && !rightKey && playerDown, closeState);
        */


        DialogManager.Instance.SetStartDialogState(initState);
    }
    private void Update()
    {
        if (playerTrapped && rightKey && TimesUsedPower % 2 == 0)
            timeTrapped += Time.unscaledDeltaTime;
        else
            timeTrapped = 0f;
    }
    private bool GetTrigger(string name) => DialogManager.Instance.HasTrigger(name);
}
