using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public static Memory Instance {  get; private set; }
    Dictionary<string, string> sceneStates = new();
    public float Playtime { get; private set; }
    private Dictionary<string, object> shards = new();
    [field: SerializeField] public Treasury Treasury { get; private set; }

    private bool counting = true;

    public int RebelRestarts { get => GetInt("totalrestarts"); set => Set("totalrestarts", value); }
    public int HighestLevel { get => GetInt("highestlevel"); set => Set("highestlevel", value); }
    public float Friend { get => GetFloat("friend"); set => Set("friend", value); }
    public void Set(string key, object value) => shards[key] = value;
    public string GetString(string key) => shards.ContainsKey(key) ? (string)Get(key) : null;
    public float GetFloat(string key) => shards.ContainsKey(key) ? (float)Get(key) : 0f;
    public int GetInt(string key) => shards.ContainsKey(key) ? (int)Get(key) : 0;
    public bool GetBool(string key) => shards.ContainsKey(key) ? (bool)Get(key) : false;
    public object Get(string key) => shards.ContainsKey(key) ? shards[key] : null;
    public string GetState(string scene)
    {
        if (!sceneStates.ContainsKey(scene))
            return null;
        return sceneStates[scene];
    }
    public void RememberState(string scene, string state)
    {
        sceneStates[scene] = state;
    }
    public void Forger()
    {
        Debug.Log("Forgor");
        sceneStates = new();
        shards = new();
        counting = true;
        Playtime = 0f;
    }
    public void STOPTHECOUNT() => counting = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        Instance = this;
        DontDestroyOnLoad(gameObject);
        counting = true;
    }
    private void Update()
    {
        if (counting)
            Playtime += Time.unscaledDeltaTime;
    }
}
