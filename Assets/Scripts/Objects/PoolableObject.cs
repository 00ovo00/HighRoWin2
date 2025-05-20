using UnityEngine;

// 스폰 대상인 오브젝트들 관리하는 인터페이스
public interface ISpawnable
{
    string PoolTag { get; }
    void OnSpawned();   // 스폰 시 활성화된 리스트에 추가
    void OnDespawned(); // 디스폰 시 활성화된 리스트에서 삭제
    void ReturnToPool();    // 디스폰하고 풀로 반환
    bool IsOnRoad();    // 오브젝트가 도로 위에 있는지 확인
}

public abstract class PoolableObject : MonoBehaviour, ISpawnable
{
    [SerializeField] protected LayerMask groundLayerMask; // Road 레이어 확인용
    
    protected string poolTag;
    
    public string PoolTag => poolTag;

    private void Awake()
    {
        groundLayerMask = LayerMask.GetMask("Road");
    }

    protected virtual void Update()
    {
        // 바닥이 없으면 풀로 반환
        if (!IsOnRoad())
        {
            ReturnToPool();
        }
    }
    
    public virtual void OnSpawned() { }
    
    public virtual void OnDespawned() { }

    public virtual void ReturnToPool() { OnDespawned(); }
    
    // Raycast로 도로 위에 있는지 확인
    public virtual bool IsOnRoad()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        return Physics.Raycast(ray, 1f, groundLayerMask);
    }
}