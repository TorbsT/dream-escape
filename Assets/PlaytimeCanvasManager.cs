using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaytimeCanvasManager : MonoBehaviour
{
    public static PlaytimeCanvasManager Instance { get; private set; }


    public bool Display { get; set; }
    [SerializeField] private TextMeshProUGUI field;


    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (!Display)
        {
            field.text = "";
            return;
        }
        if (Memory.Instance == null)
        {
            field.text = "No memory";
            return;
        }

        string playtime = Mathf.FloorToInt(Memory.Instance.Playtime).ToString();
        string friend = Memory.Instance.Friend.ToString();
        string highestLevel = Memory.Instance.HighestLevel.ToString();
        string rebelRestarts = Memory.Instance.RebelRestarts.ToString();
        string mostValuable = Memory.Instance.GetString("mostValuable");
        string seenMostValuable = Memory.Instance.GetBool("seenMostValuable") ? "Seen" : "Not seen";

        string text =
            $"Most valuable ({seenMostValuable}): {mostValuable}" +
            $"\nHighest level: {highestLevel}" +
            $"\nFriend: {friend}" +
            $"\nRebelRestarts: {rebelRestarts}" +
            $"\nPlaytime: {playtime}";
        field.text = text;
    }
}
