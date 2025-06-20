using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    private GameObject _bgmObj; // BGM 관리할 부모 오브젝트
    private GameObject _sfxObj; // SFX 관리할 부모 오브젝트

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;
    
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
        SetAudioSource();
    }

    private void Start()
    {
        LoadAudioSettings(); // 저장된 오디오 설정 로드
        PlayStartBGM(); // BGM 실행
    }
    
    private void SetAudioSource()
    {
        // 씬에서 BGM 관리하는 오브젝트 찾고 오디오소스 가져오기
        _bgmObj = GameObject.Find("@BGM");
        _bgmSource = _bgmObj.GetComponent<AudioSource>();
        
        // 씬에서 SFX 관리하는 오브젝트 찾고 오디오소스 가져오기
        _sfxObj = GameObject.Find("@SFX");
        _sfxSource = _sfxObj.GetComponent<AudioSource>();
    }

    private void PlayBGM(AudioClip clip)
    {
        if (_bgmSource.clip != clip)
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = true; // 반복 실행하도록 설정
            _bgmSource.Play();
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
        _bgmSource.volume = bgmVolume;
        _sfxSource.volume = sfxVolume;
        _bgmSource.mute = bgmMute;
        _sfxSource.mute = sfxMute;
    }
    
    // 현재 오디오 설정을 PlayerPrefs에 저장
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(BGMVolumeKey, _bgmSource.volume);
        PlayerPrefs.SetFloat(SFXVolumeKey, _sfxSource.volume);
        PlayerPrefs.SetInt(BGMMuteKey, _bgmSource.mute ? 1 : 0);
        PlayerPrefs.SetInt(SFXMuteKey, _sfxSource.mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 볼륨 조절 및 토글
    public float GetBGMVolume() { return _bgmSource.volume; }
    public void SetBGMVolume(float volume) 
    { 
        _bgmSource.volume = volume;
        SaveAudioSettings();
    }
    public float GetSFXVolume() { return _sfxSource.volume; }
    public void SetSFXVolume(float volume) 
    { 
        _sfxSource.volume = volume;
        SaveAudioSettings();
    }
    
    public void ToggleBGM() 
    { 
        _bgmSource.mute = !_bgmSource.mute;
        SaveAudioSettings();
    }
    
    public void ToggleSFX() 
    { 
        _sfxSource.mute = !_sfxSource.mute;
        SaveAudioSettings();
    }
    
    // 음소거 상태 확인 메서드 추가
    public bool IsBGMMuted() { return _bgmSource.mute; }
    public bool IsSFXMuted() { return _sfxSource.mute; }
    
    // 오디오 클립 재생
    public void PlaySFX(AudioClip clip) { _sfxSource.PlayOneShot(clip); }
    public void PlayStartBGM() => PlayBGM(bgmClip);
    public void PlayClickSFX() => PlaySFX(clickSfx);
    public void PlayJumpSFX() => PlaySFX(jumpSfx);
    public void PlayItemSFX() => PlaySFX(itemSfx);
    public void PlayCollsionSFX() => PlaySFX(collisionSfx);
    public void PlayBlockedSFX() => PlaySFX(blockedSfx);
    public void PlayAchieveSFX() => PlaySFX(achieveSfx);
    public void PlayMoveSFX() => PlaySFX(moveSfx);
}