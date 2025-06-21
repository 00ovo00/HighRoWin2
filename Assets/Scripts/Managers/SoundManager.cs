using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : SingletonBase<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private GameObject bgmObj; // BGM 관리할 부모 오브젝트
    [SerializeField] private GameObject sfxObj; // SFX 관리할 부모 오브젝트
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("BGM")]
    [SerializeField] private AudioClip bgmClip;

    [Header("SFX")]
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip itemSfx;
    [SerializeField] private AudioClip collisionSfx;
    [SerializeField] private AudioClip blockedSfx;
    [SerializeField] private AudioClip achieveSfx;
    [SerializeField] private AudioClip moveSfx;
    
    // PlayerPrefs 키 상수
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string BGMMuteKey = "BGMMute";
    private const string SFXMuteKey = "SFXMute";
    
    // 기본값
    private const float DefaultBGMVolume = 0.8f;
    private const float DefaultSFXVolume = 1.0f;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        // 오디오 소스 없으면 만들어서 연결
        if (bgmObj == null)
        {
            bgmObj = new GameObject();
            bgmObj.name = "@BGM";
            bgmObj.transform.SetParent(transform);
            bgmObj.AddComponent<AudioSource>();
        }
        if (sfxObj == null)
        {
            sfxObj = new GameObject();
            sfxObj.name = "@SFX";
            sfxObj.transform.SetParent(transform);
            sfxObj.AddComponent<AudioSource>();
        }
        if (bgmSource == null)
            bgmSource = bgmObj.GetComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = sfxObj.GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadAudioSettings(); // 저장된 오디오 설정 로드
        PlayStartBGM(); // BGM 실행
    }

    private void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // 반복 실행하도록 설정
            bgmSource.Play();
        }
    }
    
    // PlayerPrefs에서 오디오 설정 로드
    private void LoadAudioSettings()
    {
        // 볼륨 설정 로드 (저장된 값이 없으면 기본값 사용)
        float bgmVolume = PlayerPrefs.GetFloat(BGMVolumeKey, DefaultBGMVolume);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, DefaultSFXVolume);
        
        // 음소거 설정 로드 (저장된 값이 없으면 false)
        bool bgmMute = PlayerPrefs.GetInt(BGMMuteKey, 0) == 1;
        bool sfxMute = PlayerPrefs.GetInt(SFXMuteKey, 0) == 1;
        
        // 오디오 소스에 적용
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
        bgmSource.mute = bgmMute;
        sfxSource.mute = sfxMute;
    }
    
    // 현재 오디오 설정을 PlayerPrefs에 저장
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(BGMVolumeKey, bgmSource.volume);
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxSource.volume);
        PlayerPrefs.SetInt(BGMMuteKey, bgmSource.mute ? 1 : 0);
        PlayerPrefs.SetInt(SFXMuteKey, sfxSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 볼륨 조절 및 토글
    public float GetBGMVolume() { return bgmSource.volume; }
    public void SetBGMVolume(float volume) 
    { 
        bgmSource.volume = volume;
        SaveAudioSettings();
    }
    public float GetSFXVolume() { return sfxSource.volume; }
    public void SetSFXVolume(float volume) 
    { 
        sfxSource.volume = volume;
        SaveAudioSettings();
    }
    
    public void ToggleBGM() 
    { 
        bgmSource.mute = !bgmSource.mute;
        SaveAudioSettings();
    }
    
    public void ToggleSFX() 
    { 
        sfxSource.mute = !sfxSource.mute;
        SaveAudioSettings();
    }
    
    // 음소거 상태 확인 메서드 추가
    public bool IsBGMMuted() { return bgmSource.mute; }
    public bool IsSFXMuted() { return sfxSource.mute; }
    
    // 오디오 클립 재생
    public void PlaySFX(AudioClip clip) { sfxSource.PlayOneShot(clip); }
    public void PlayStartBGM() => PlayBGM(bgmClip);
    public void PlayClickSFX() => PlaySFX(clickSfx);
    public void PlayJumpSFX() => PlaySFX(jumpSfx);
    public void PlayItemSFX() => PlaySFX(itemSfx);
    public void PlayCollsionSFX() => PlaySFX(collisionSfx);
    public void PlayBlockedSFX() => PlaySFX(blockedSfx);
    public void PlayAchieveSFX() => PlaySFX(achieveSfx);
    public void PlayMoveSFX() => PlaySFX(moveSfx);
}