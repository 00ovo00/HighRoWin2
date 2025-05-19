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
    [SerializeField] private AudioClip collisionSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip itemSfx;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        SetAudioSource();
    }

    private void Start()
    {
        // 오디오 초기 볼륨 설정
        _bgmSource.volume = 0.8f;
        _sfxSource.volume = 1.0f;
        
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

    // 볼륨 조절 및 토글
    public float GetBGMVolume() { return _bgmSource.volume; }
    public void SetBGMVolume(float volume) { _bgmSource.volume = volume; }
    public float GetSFXVolume() { return _sfxSource.volume; }
    public void SetSFXVolume(float volume) { _sfxSource.volume = volume; }
    public void ToggleBGM() { _bgmSource.mute = !_bgmSource.mute; }
    public void ToggleSFX() { _sfxSource.mute = !_sfxSource.mute; }
    
    // 오디오 클립 재생
    public void PlaySFX(AudioClip clip) { _sfxSource.PlayOneShot(clip); }
    public void PlayStartBGM() => PlayBGM(bgmClip);
    public void PlayCollsionSFX() => PlaySFX(collisionSfx);
    public void PlayMoveSFX() => PlaySFX(jumpSfx);
    public void PlayItemSFX() => PlaySFX(itemSfx);
}