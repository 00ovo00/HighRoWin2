using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{
    private GameObject _bgmObj;
    private GameObject _sfxObj;

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
        
        SetAudioSource();
    }

    private void Start()
    {
        _bgmSource.volume = 0.8f;
        _sfxSource.volume = 1.0f;
        
        PlayStartBGM();
    }
    
    private void SetAudioSource()
    {
        _bgmObj = GameObject.Find("@BGM");
        _bgmSource = _bgmObj.GetComponent<AudioSource>();
        
        _sfxObj = GameObject.Find("@SFX");
        _sfxSource = _sfxObj.GetComponent<AudioSource>();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (_bgmSource.clip != clip)
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
    }
    
    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
    
    public void PlayStartBGM() => PlayBGM(bgmClip);
    public void PlayCollsionSFX() => PlaySFX(collisionSfx);
    public void PlayMoveSFX() => PlaySFX(jumpSfx);
    public void PlayItemSFX() => PlaySFX(itemSfx);
}