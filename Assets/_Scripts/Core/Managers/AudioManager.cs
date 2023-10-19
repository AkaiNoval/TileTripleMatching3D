using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------AUDIO SOURCE-----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [Header("----------AUDIO CLIP-----------")]
    [SerializeField] private List<AudioFile> audioFiles;
    private void Start()
    {
        PlayMusicBackGround();
        InitSFXManager();
    }
    void PlayMusicBackGround()
    {
        AudioFile musicAudioFile = audioFiles.Find(file => file.Key == AudioKey.Music);
        /* Check if the musicAudioFile is found and its clip is not null */
        if (musicAudioFile != null && musicAudioFile.Clip != null)
        {
            /* Set the musicSource's clip to the found audio clip */
            musicSource.clip = musicAudioFile.Clip;
            /* Play the music */
            musicSource.Play();
        }
        else
        {
            Debug.LogError("Music audio clip not found or is null.");
        }
    }
    void InitSFXManager()
    {
        AudioSFXManager.InitializeSFXActions(SFXSource, audioFiles);
    }
}
[Serializable]
public class AudioFile
{
    public AudioClip Clip;
    public AudioKey Key;
}
public enum AudioKey
{
    Music,
    WinSFX,
    LostSFX,
    Matching,
    SlotSort,
    Kick,
    SlowOnTime,
}
public class AudioSFXManager
{
    private static AudioSource sfxSource;
    private static Dictionary<AudioKey, AudioClip> sfxClips;

    public static void InitializeSFXActions(AudioSource source, List<AudioFile> audioFiles)
    {
        sfxSource = source;
        sfxClips = new Dictionary<AudioKey, AudioClip>();

        foreach (var audioFile in audioFiles)
        {
            sfxClips[audioFile.Key] = audioFile.Clip;
        }
    }

    public static void PlaySFX(AudioKey key)
    {
        if (sfxSource != null && sfxClips.ContainsKey(key))
        {
            sfxSource.PlayOneShot(sfxClips[key]);
        }
        else
        {
            Debug.LogError("SFX audio source not set or SFX clip not found.");
        }
    }
}
