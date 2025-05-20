using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [SerializeField] private Button closeButton; // X 버튼(멈춘 상태에서 다시 재개)
    [SerializeField] private Button BGMButton;  // 배경음 토글 버튼
    [SerializeField] private Button SFXButton;  // 효과음 토글 버튼

    [SerializeField] private Slider BGMSlider;  // 배경음 조절 슬라이더
    [SerializeField] private Slider SFXSlider;  // 효과음 조절 슬라이더

    private void Awake()
    {
        // 슬라이더 기본 세팅 값에 맞춰 조절
        BGMSlider.value = SoundManager.Instance.GetBGMVolume();
        SFXSlider.value = SoundManager.Instance.GetSFXVolume();
    }

    private void OnEnable()
    {
        // 닫기 버튼 누르면 설정창 닫기
        closeButton.onClick.AddListener(() => UIManager.Instance.Hide<SettingPopup>());
        // 버튼 누르면 소리 토글
        BGMButton.onClick.AddListener(SoundManager.Instance.ToggleBGM);
        SFXButton.onClick.AddListener(SoundManager.Instance.ToggleSFX);
        // 슬라이더 값 변하면 볼륨 변경하도록 이벤트 연결
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
