using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{

    public static MusicSystem Instance { get; private set; }
    [SerializeField, Range(0.1f, 10f)] private float REDpitch = 0.5f;
    [SerializeField, Range(0.1f, 10f)] private float BLUpitch = 1.5f;
    [SerializeField] private float transitionTime = 1f;

    [Range(0f, 1f)] private float progress = 0f;
    private float goal = 1f;
    [SerializeField] private AudioSource BLUsource;
    [SerializeField] private AudioSource REDsource;
    [SerializeField] private AudioReverbPreset REDreverb;
    [SerializeField] private AudioReverbPreset BLUreverb;
    private AudioReverbZone zone;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        zone = GetComponent<AudioReverbZone>();
        goal = 0f;
        progress = goal;
    }
    private void Update()
    {
        float d = Time.deltaTime / transitionTime;
        if (progress > goal)
            progress -= d;
        else if (progress < goal)
            progress += d;
        progress = Mathf.Clamp(progress, 0f, 1f);

        float REDvolume = progress;
        float BLUvolume = 1f - progress;
        float pitch = Mathf.Lerp(BLUpitch, REDpitch, progress);

        float settingsMod = PauseMenu.Instance.Master * PauseMenu.Instance.Music;
        BLUsource.pitch = pitch;
        BLUsource.volume = BLUvolume * settingsMod;
        REDsource.pitch = pitch;
        REDsource.volume = REDvolume * settingsMod;
    }
    public void UsedAbility(bool RED, bool initial)
    {
        goal = RED ? 0f : 1f;
        zone.reverbPreset = RED ? BLUreverb : REDreverb;
    }
}
