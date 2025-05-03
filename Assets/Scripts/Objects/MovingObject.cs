using UnityEngine;

public class MovingObject : PoolableObject
{
    private float speed;
    private float direction;
    
    public void Initialize(string tag, float direction, float speed)
    {
        this.poolTag = tag;
        this.direction = direction;
        this.speed = speed;
    }
    
    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * direction * speed * Time.fixedDeltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.IsPlaying) return;    // 이미 게임오버 된 상황에서는 실행 X
        
        if (other.CompareTag("Player"))  // 플레이어와 트리거되면
        {
            Animator anim = other.gameObject.GetComponentInChildren<Animator>();
            anim.SetBool("IsDead", true);   // 죽는 애니메이션 재생
            SoundManager.Instance.PlayCollsionSFX();    // 충돌 효과음 재생
            GameManager.Instance.GameOver();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // 좌 -> 우 이동, 오른쪽 벽에 트리거되고, 활성화된 상태면
        if (direction < 0 && other.CompareTag("RightWall") && isActiveAndEnabled)
        {
            ReturnToPool();
        }
        // 우 -> 좌 이동, 왼쪽 벽에 트리거되고, 활성화된 상태면
        if (direction > 0 && other.CompareTag("LeftWall") && isActiveAndEnabled)
        {
            ReturnToPool();
        }   
    }
}