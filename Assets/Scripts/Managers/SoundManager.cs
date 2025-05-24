using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    private GameObject _bgmObj; // parent object that manages BGM
    private GameObject _sfxObj; // parent object that manages SFX

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
        // initialize audio volume
        _bgmSource.volume = 0.8f;
        _sfxSource.volume = 1.0f;
        
        PlayStartBGM();
    }
    
    private void SetAudioSource()
    {
        // find object that manages BGM in the scene and get audio source
        _bgmObj = GameObject.Find("@BGM");
        _bgmSource = _bgmObj.GetComponent<AudioSource>();
        
        // find object that manages SFX in the scene and get audio source
        _sfxObj = GameObject.Find("@SFX");
        _sfxSource = _sfxObj.GetComponent<AudioSource>();
    }

    private void PlayBGM(AudioClip clip)
    {
        if (_bgmSource.clip != clip)
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = true; // play repeatedly
            _bgmSource.Play();
        }
    }

    // control and toggle volume
    public float GetBGMVolume() { return _bgmSource.volume; }
    public void SetBGMVolume(float volume) { _bgmSource.volume = volume; }
    public float GetSFXVolume() { return _sfxSource.volume; }
    public void SetSFXVolume(float volume) { _sfxSource.volume = volume; }
    public void ToggleBGM() { _bgmSource.mute = !_bgmSource.mute; }
    public void ToggleSFX() { _sfxSource.mute = !_sfxSource.mute; }
    
    // play audio clip
    public void PlaySFX(AudioClip clip) { _sfxSource.PlayOneShot(clip); }
    public void PlayStartBGM() => PlayBGM(bgmClip);
    public void PlayCollsionSFX() => PlaySFX(collisionSfx);
    public void PlayMoveSFX() => PlaySFX(jumpSfx);
    public void PlayItemSFX() => PlaySFX(itemSfx);
}