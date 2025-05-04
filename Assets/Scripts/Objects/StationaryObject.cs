using UnityEngine;

public class StationaryObject : PoolableObject
{
    public void Initialize(string tag)
    {
        this.poolTag = tag;
    }
    
    public override void OnSpawned()
    {
        PlaySceneManager.Instance.activeStationaryObjects.Add(this);
    }
    
    public override void OnDespawned()
    {
        PlaySceneManager.Instance.activeStationaryObjects.Remove(this);
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        PoolManager.Instance.ReturnToPool(poolTag, this);
    }
}