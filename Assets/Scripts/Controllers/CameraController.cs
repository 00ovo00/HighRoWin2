using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float backDistance;    // 카메라 줌아웃하는 거리
    private Vector3 _offset;    // 카메라와 플레이어 사이의 거리

    private void Awake()
    {
        backDistance = 3;
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

    public void CameraZoomOut()
    {
        _offset -= transform.forward * backDistance;
    }
}