using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Settings")]
public class SettingsObject : ScriptableObject
{
    [field: SerializeField, Range(0f, 1f)] public float MasterVolume = 1f; 
    [field: SerializeField, Range(0f, 1f)] public float MusicVolume = 1f; 
    [field: SerializeField, Range(0f, 1f)] public float SFXVolume = 1f; 
}
