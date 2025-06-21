using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem glowEffect; // 랜턴 파티클 효과

    private void Awake()
    {
        if (glowEffect == null)
            glowEffect = GetComponent<ParticleSystem>();
    }
    
    private void Start()
    {
        if (glowEffect != null) glowEffect.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && glowEffect != null)
        {
            glowEffect.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && glowEffect != null)
        {
            glowEffect.Stop();
        }
    }
}