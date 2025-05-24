using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [SerializeField] private Button closeButton; // X button(resume)
    [SerializeField] private Button BGMButton;  // BGM toggle
    [SerializeField] private Button SFXButton;  // SFX toggle

    [SerializeField] private Slider BGMSlider;  // BGM control
    [SerializeField] private Slider SFXSlider;  // SFX control

    private void Awake()
    {
        // Adjust to slider default settings
        BGMSlider.value = SoundManager.Instance.GetBGMVolume();
        SFXSlider.value = SoundManager.Instance.GetSFXVolume();
    }

    private void OnEnable()
    {
        // when close button clicked, hide setting pop-up
        closeButton.onClick.AddListener(() => UIManager.Instance.Hide<SettingPopup>());
        // when toggle button clicked, toggle sound
        BGMButton.onClick.AddListener(SoundManager.Instance.ToggleBGM);
        SFXButton.onClick.AddListener(SoundManager.Instance.ToggleSFX);
        // connect events to change volume when slider values change
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
