using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float backDistance;    // camera zoom out distance
    private Vector3 _offset;    // distance between player and camera

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
            // keep distance based on player position while moving
            transform.position = Vector3.Lerp(transform.position, player.position + _offset, 0.1f);
        }
    }

    public void CameraZoomOut()
    {
        _offset -= transform.forward * backDistance;
    }
}