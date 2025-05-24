using UnityEngine;

// interface to manage spawned objects 
public interface ISpawnable
{
    string PoolTag { get; }
    void OnSpawned();   // add to active object list when spawned
    void OnDespawned(); // remove from active object list when despawned
    void ReturnToPool();    // despawn and return the object to the pool
    bool IsOnRoad();    // check the object is on a road or not
}

public abstract class PoolableObject : MonoBehaviour, ISpawnable
{
    [SerializeField] protected LayerMask groundLayerMask;   // Road
    
    protected string poolTag;
    
    public string PoolTag => poolTag;

    private void Awake()
    {
        groundLayerMask = LayerMask.GetMask("Road");
    }

    protected virtual void Update()
    {
        // if it is not on a road, return it to the pool
        if (!IsOnRoad())
        {
            ReturnToPool();
        }
    }
    
    public virtual void OnSpawned() { }
    
    public virtual void OnDespawned() { }

    public virtual void ReturnToPool() { OnDespawned(); }
    
    // check it is on a road by using raycast
    public virtual bool IsOnRoad()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        return Physics.Raycast(ray, 1f, groundLayerMask);
    }
}