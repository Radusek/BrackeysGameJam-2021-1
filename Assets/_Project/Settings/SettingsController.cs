using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private float appliedSFXValue;
    private float appliedMusicValue;
    private bool isDirty;


    private void Awake()
    {
        sfxSlider.value = SaveManager.Get<float>("SFXVolume");
        musicSlider.value = SaveManager.Get<float>("MusicVolume");
    }

    private void OnEnable() => SetCurrentValues();

    private void OnDisable()
    {
        if (isDirty)
            Cancel();
    }

    private void SetCurrentValues()
    {
        appliedSFXValue = sfxSlider.value;
        appliedMusicValue = musicSlider.value;
    }

    public void Apply()
    {
        SetCurrentValues();
        SaveManager.Set("SFXVolume", appliedSFXValue);
        SaveManager.Set("MusicVolume", appliedMusicValue);
        isDirty = false;
    }

    public void Cancel()
    {
        sfxSlider.value = appliedSFXValue;
        musicSlider.value = appliedMusicValue;
        AudioManager.Instance.SFXVolume = appliedSFXValue;
        AudioManager.Instance.SetMusicVolume(appliedMusicValue);
        isDirty = false;
    }

    public void OnSFXVolumeChanged(float newValue)
    {
        AudioManager.Instance.SFXVolume = newValue;
        isDirty = true;
    }

    public void OnMusicVolumeChanged(float newValue)
    {
        AudioManager.Instance.SetMusicVolume(newValue);
        isDirty = true;
    }
}
