using UnityEngine;

public interface IPoolable
{
    string PoolTag { get; }
    void OnSpawned();
    void OnDespawned();
}

public interface ISpawnable : IPoolable
{
    void ReturnToPool();
    bool IsOnRoad();
}

public abstract class PoolableObject : MonoBehaviour, ISpawnable
{
    [SerializeField] protected LayerMask groundLayerMask; // Road 레이어 확인용
    
    protected string poolTag;
    
    public string PoolTag => poolTag;
    
    protected virtual void Update()
    {
        // 바닥이 없으면 풀로 반환
        if (!IsOnRoad())
        {
            ReturnToPool();
        }
    }
    
    public virtual void OnSpawned()
    {
        PlaySceneManager.Instance.AddActiveList(this);
    }
    
    public virtual void OnDespawned()
    {
        PlaySceneManager.Instance.RemoveActiveList(this);
    }
    
    public virtual void ReturnToPool()
    {
        OnDespawned();
        PoolManager.Instance.ReturnToPool(poolTag, this);
    }
    
    // Raycast로 도로 위에 있는지 확인
    public virtual bool IsOnRoad()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        return Physics.Raycast(ray, 1f, groundLayerMask);
    }
}