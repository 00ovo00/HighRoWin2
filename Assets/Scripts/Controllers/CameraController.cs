using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 _offset;

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver -= () => { _offset += Vector3.back; };
        GameManager.Instance.OnGameOver += () => { _offset += Vector3.back; };
    }
    
    private void Start()
    {
        _offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // 플레이어 위치 기준으로 일정 거리 유지하며 이동
            transform.position = Vector3.Lerp(transform.position, player.position + _offset, 0.1f);
        }
    }
}