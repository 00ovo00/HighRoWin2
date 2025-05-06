using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button BGMButton;
    [SerializeField] private Button SFXButton;

    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        BGMSlider.value = SoundManager.Instance.GetBGMVolume();
        SFXSlider.value = SoundManager.Instance.GetSFXVolume();
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(() => UIManager.Instance.Hide<SettingPopup>());
        BGMButton.onClick.AddListener(SoundManager.Instance.ToggleBGM);
        SFXButton.onClick.AddListener(SoundManager.Instance.ToggleSFX);
        BGMSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        SFXSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
}
