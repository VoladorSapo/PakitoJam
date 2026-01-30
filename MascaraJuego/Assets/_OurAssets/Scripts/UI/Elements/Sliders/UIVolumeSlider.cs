using System;
using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    [Inject] GameSettings settings;

    private enum VolumeSliderType { Master, SFX, Music }

    [SerializeField] VolumeSliderType sliderType;
    [SerializeField] Slider slider;

    private Dictionary<VolumeSliderType, Func<float>> getters;
    private Dictionary<VolumeSliderType, Action<float>> setters;

    private void Awake()
    {

        getters = new()
        {
            { VolumeSliderType.Master, () => settings.MasterVolume },
            { VolumeSliderType.SFX,    () => settings.SFXVolume },
            { VolumeSliderType.Music,  () => settings.MusicVolume }
        };

        setters = new()
        {
            { VolumeSliderType.Master, v => settings.SetMasterVolume(v) },
            { VolumeSliderType.SFX,    v => settings.SetSFXVolume(v) },
            { VolumeSliderType.Music,  v => settings.SetMusicVolume(v) }
        };
    }

    private void OnEnable()
    {
        slider.SetValueWithoutNotify(getters[sliderType]());
    }

    public void SetVolume()
    {
        setters[sliderType](slider.value);
    }
}

