using System;
using System.Collections;
using System.Collections.Generic;
using TorbuTils.EzPools;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Serializable]
    private class Sound
    {
        public string name;
        public AudioClip clip;
    }
    private class LiveSound
    {
        public AudioSource source;
        public float timeRemaining;
    }

    [SerializeField] private List<Sound> sounds = new();
    [SerializeField] private AudioSource prefab;
    private List<LiveSound> liveSounds = new();

    public void PlayRandom(string name, float volume)
    {
        List<Sound> list = sounds.FindAll(x => x.name.StartsWith(name));
        int i = UnityEngine.Random.Range(0, list.Count);
        Play(list[i].name, volume);
    }
    public void Play(string name) => Play(name, 1f);
    public void Play(string name, float volume) => Play(name, volume, Vector2.zero);
    public void Play(string name, float volume, Vector2 position)
    {
        if (!Application.isPlaying) return;
        Sound p = sounds.Find(x => x.name == name);
        if (p == null)
        {
            Debug.LogWarning($"'{name}' is not a registered sound");
            p = sounds[0];
        }
        GameObject go = Pools.Instance.Depool(prefab.gameObject);
        go.transform.position = position;
        AudioSource source = go.GetComponent<AudioSource>();
        AudioClip clip = p.clip;
        source.clip = clip;
        source.volume = volume;
        if (clip == null) Debug.LogWarning($"{name} has null clip");
        LiveSound liveSound = new();
        liveSound.source = source;
        liveSound.timeRemaining = clip.length;
        source.pitch = 1f + UnityEngine.Random.Range(-0.1f, 0.1f);
        source.Play();
        liveSounds.Add(liveSound);
    }

    private void Awake()
    {
        Instance = this;
        if (Instance != null) Destroy(gameObject);
    }
    private void Update()
    {
        for (int i = liveSounds.Count - 1; i >= 0; i--)
        {
            LiveSound liveParticle = liveSounds[i];
            liveParticle.timeRemaining -= Time.deltaTime;
            if (liveParticle.timeRemaining < 0f)
            {
                liveSounds.RemoveAt(i);
                Pools.Instance.Enpool(liveParticle.source.gameObject);
            }
        }
    }

}
