using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : UIBase
{
    [SerializeField] private Button closeButton; // X 버튼(멈춘 상태에서 다시 재개)
    [SerializeField] private Button BGMButton;  // 배경음 토글 버튼
    [SerializeField] private Button SFXButton;  // 효과음 토글 버튼

    [SerializeField] private Slider BGMSlider;  // 배경음 조절 슬라이더
    [SerializeField] private Slider SFXSlider;  // 효과음 조절 슬라이더
    
    [Header("Toggle Button Images")]
    [SerializeField] private Image BGMButtonImage;    // BGM 버튼 이미지
    [SerializeField] private Image SFXButtonImage;    // SFX 버튼 이미지
    [SerializeField] private Sprite BGMUnmuteSprite;  // BGM 활성 상태 스프라이트
    [SerializeField] private Sprite SFXUnmuteSprite;  // SFX 활성 상태 스프라이트
    [SerializeField] private Sprite BGMMuteSprite;    // BGM 음소거 상태 스프라이트
    [SerializeField] private Sprite SFXMuteSprite;    // SFX 음소거 상태 스프라이트

    private void OnEnable()
    {
        InitializeUI(); // UI 상태 새로고침
        UnsubscribeEvent();
        
        // 닫기 버튼 누르면 설정창 닫기
        closeButton.onClick.AddListener(OnCloseButtonClick);
        
        // BGM 토글 버튼
        BGMButton.onClick.AddListener(OnBGMButtonClick);
        
        // SFX 토글 버튼
        SFXButton.onClick.AddListener(OnSFXButtonClick);
        
        // 슬라이더 값 변경 이벤트
        BGMSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        SFXSlider.onValueChanged.AddListener(OnSFXVolumeChanged);    
    }
    
    private void UnsubscribeEvent()
    {
        closeButton.onClick.RemoveAllListeners();
        BGMButton.onClick.RemoveAllListeners();
        SFXButton.onClick.RemoveAllListeners();
        BGMSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.RemoveAllListeners();
    }
    
    // UI 초기화, 저장된 설정값으로 슬라이더와 버튼 상태를 설정
    private void InitializeUI()
    {
        // 슬라이더 값을 저장된 볼륨으로 설정
        BGMSlider.value = SoundManager.Instance.GetBGMVolume();
        SFXSlider.value = SoundManager.Instance.GetSFXVolume();
        
        UpdateButtonImages();
    }
    
    // 음소거 상태에 따라 버튼 이미지 업데이트
    private void UpdateButtonImages()
    {
        BGMButtonImage.sprite = SoundManager.Instance.IsBGMMuted() ? BGMMuteSprite : BGMUnmuteSprite;
        SFXButtonImage.sprite = SoundManager.Instance.IsSFXMuted() ? SFXMuteSprite : SFXUnmuteSprite;
    }
    
    // 닫기 버튼 누르면 실행
    private void OnCloseButtonClick()
    {
        SoundManager.Instance.PlayClickSFX();
        UIManager.Instance.Hide<SettingPopup>();
    }
    
    // BGM 토글 버튼 누르면 실행
    private void OnBGMButtonClick()
    {
        SoundManager.Instance.PlayClickSFX();
        SoundManager.Instance.ToggleBGM();
        UpdateButtonImages();
    }
    
    // SFX 토글 버튼 누르면 실행
    private void OnSFXButtonClick()
    {
        SoundManager.Instance.PlayClickSFX();
        SoundManager.Instance.ToggleSFX();
        UpdateButtonImages();
    }

    // BGM 볼륨 변경 처리
    private void OnBGMVolumeChanged(float value) { SoundManager.Instance.SetBGMVolume(value); }
    
    // SFX 볼륨 변경 처리
    private void OnSFXVolumeChanged(float value) { SoundManager.Instance.SetSFXVolume(value); }
}