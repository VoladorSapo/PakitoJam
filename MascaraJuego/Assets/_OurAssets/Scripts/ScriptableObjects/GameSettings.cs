using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Game/Settings")]
public class GameSettings : ScriptableObject
{
    [HorizontalLine(color: EColor.Green)]
    [Header("Necessary References")]
    [SerializeField] AudioMixer gameMixer;
    [SerializeField] VolumeProfile globalVolumePorfile;
    
    [HorizontalLine(color: EColor.Green)]
    [Header("Volume Settings")]
    [SerializeField] float masterVolume;
    [SerializeField] float sfxVolume;
    [SerializeField] float musicVolume;
    
    public float MasterVolume => masterVolume;
    public float SFXVolume => sfxVolume;
    public float MusicVolume => musicVolume;
    
    [Header("Colorblind Settings")]
    [SerializeField] int currentColorblindMode = 0;
    [SerializeField] Texture2D[] colorblindLutTextures;
    public int CurrentColorblindMode => currentColorblindMode;
    
    public void SetMasterVolume(float newMasterVolume)
    {
        masterVolume = Mathf.Clamp01(newMasterVolume);
        gameMixer.SetFloat("MasterVolume", ConversionHelper.Linear01ToDecibels(newMasterVolume));
    }
    public void SetSFXVolume(float newSfxVolume)
    {
        sfxVolume = Mathf.Clamp01(newSfxVolume);
        gameMixer.SetFloat("SFXVolume", ConversionHelper.Linear01ToDecibels(newSfxVolume));
    }
    public void SetMusicVolume(float newMusicVolume)
    {
        musicVolume = Mathf.Clamp01(newMusicVolume);
        gameMixer.SetFloat("MusicVolume", ConversionHelper.Linear01ToDecibels(newMusicVolume));
    }
    
    public void SetColorblindMode(int newMode)
    {
        Debug.Log($"Changing colorblind mode to {newMode}");
        currentColorblindMode = newMode;
        if (globalVolumePorfile.TryGet(out ColorLookup lookup))
        {
            Texture2D texture = newMode > 0 ? colorblindLutTextures[newMode - 1] : null;
            lookup.texture.value = texture;
        }
    }
    
    public void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetInt("ColorblindMode", currentColorblindMode);
    }
    
    public void Load(bool defaultLoad = false)
    {
        SetMasterVolume(!defaultLoad ? PlayerPrefs.GetFloat("MasterVolume", 0.8f) : masterVolume);
        SetSFXVolume(!defaultLoad ? PlayerPrefs.GetFloat("SFXVolume", 0.8f) : sfxVolume);
        SetMusicVolume(!defaultLoad ? PlayerPrefs.GetFloat("MusicVolume", 0.8f) : musicVolume);
        
        SetColorblindMode(!defaultLoad ? PlayerPrefs.GetInt("ColorblindMode", 0) : currentColorblindMode);
    }
    
}