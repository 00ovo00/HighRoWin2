using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem _glowEffect; // 랜턴 파티클 효과

    private void Awake()
    {
        _glowEffect = GetComponent<ParticleSystem>();
    }
    
    private void Start()
    {
        if (_glowEffect != null) _glowEffect.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _glowEffect != null)
        {
            _glowEffect.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _glowEffect != null)
        {
            _glowEffect.Stop();
        }
    }
}