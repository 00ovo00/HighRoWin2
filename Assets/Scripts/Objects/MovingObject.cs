using UnityEngine;

public class MovingObject : MonoBehaviour
{ 
    private float _speed;
    private float _direction;
    
    [SerializeField] private LayerMask layerMask; // 확인할 레이어 마스크(Road로 설정)

    public void Initialize(float movementDirection, float objectSpeed)
    {
        // 이동 방향과 속력 초기화
        _direction = movementDirection;
        _speed = objectSpeed;
    }
    
    private void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 1, layerMask))
        {
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(name, this);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * _direction * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.IsPlaying) return;    // 이미 게임오버 된 상황에서는 실행 X
        
        if (other.tag == "Player")  // 플레이어와 트리거되면
        {
            //Debug.Log("Player entered");
            Animator anim = other.gameObject.GetComponentInChildren<Animator>();
            anim.SetBool("IsDead", true);   // 죽는 애니메이션 재생
            SoundManager.Instance.PlayCollsionSFX();    // 충돌 효과음 재생
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 좌 -> 우 이동, 오른쪽 벽에 트리거되고, 활성화된 상태면
        if (_direction < 0 && other.CompareTag("RightWall") && isActiveAndEnabled)
        {
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(name, this);
        }
        // 우 -> 좌 이동, 왼쪽 벽에 트리거되고, 활성화된 상태면
        if (_direction > 0 && other.CompareTag("LeftWall") && isActiveAndEnabled)
        {
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(name, this);
        }   
    }
}