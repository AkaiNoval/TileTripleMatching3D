using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void SetMusicValueToMixer()
    {
        VolumeSettings.musicVolumeChanged?.Invoke(slider.value);
    }
    public void SetSFXValueToMixer()
    {
        VolumeSettings.SFXVolumeChanged?.Invoke(slider.value);
    }
}
