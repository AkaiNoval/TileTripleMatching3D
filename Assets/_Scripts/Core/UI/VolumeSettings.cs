using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public static Action<float> musicVolumeChanged;
    public static Action<float> SFXVolumeChanged;

    [SerializeField] private AudioMixer myMixer;

    private const string MUSICVOLUMEPARAM = "musicVolume";
    private const string SFXVOLUMEPARAM = "SFXVolume";

    private void Start()
    {
        musicVolumeChanged = SetMusicVolume;
        SFXVolumeChanged = SetSFXVolume;
    }
    private void OnDestroy()
    {
        musicVolumeChanged = null;
        SFXVolumeChanged = null;
    }
    public void SetMusicVolume(float volume)
    {
        myMixer.SetFloat(MUSICVOLUMEPARAM, Mathf.Log10(volume) * 25f);
    }
    public void SetSFXVolume(float volume)
    {
        myMixer.SetFloat(SFXVOLUMEPARAM, Mathf.Log10(volume) * 25f);
    }


}
