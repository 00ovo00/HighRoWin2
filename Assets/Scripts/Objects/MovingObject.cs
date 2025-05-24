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
    
    public override void OnSpawned()
    {
        PlaySceneManager.Instance.activeMovingObjects.Add(this);
    }
    
    public override void OnDespawned()
    {
        PlaySceneManager.Instance.activeMovingObjects.Remove(this);
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        PoolManager.Instance.ReturnToPool(poolTag, this);
    }
    
    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * (direction * speed * Time.fixedDeltaTime));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isPlaying) return;    // return if game is not playing
        
        if (other.CompareTag("Player"))
        {
            Animator anim = other.gameObject.GetComponentInChildren<Animator>();
            anim.SetBool("IsDead", true);
            SoundManager.Instance.PlayCollsionSFX();
            CameraController camera = FindAnyObjectByType<CameraController>();
            camera.CameraZoomOut();
            GameManager.Instance.GameOver();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // move left to right
        if (direction < 0 && other.CompareTag("RightWall") && isActiveAndEnabled)
        {
            ReturnToPool();
        }
        // move right to left
        if (direction > 0 && other.CompareTag("LeftWall") && isActiveAndEnabled)
        {
            ReturnToPool();
        }   
    }
}