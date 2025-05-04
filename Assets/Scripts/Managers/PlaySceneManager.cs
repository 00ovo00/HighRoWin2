using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaySceneManager : SingletonBase<PlaySceneManager>
{
    [SerializeField] private PoolManager.PoolConfig[] poolConfigs;
    public List<Item> activeItems = new List<Item>();
    public List<MovingObject> activeMovingObjects = new List<MovingObject>();
    public List<StationaryObject> activeStationaryObjects = new List<StationaryObject>();
    
    protected override void Awake()
    {
        base.Awake();
        PoolManager.Instance.AddPools<Item>(poolConfigs);
        PoolManager.Instance.AddPools<MovingObject>(poolConfigs);
        PoolManager.Instance.AddPools<StationaryObject>(poolConfigs);
    }

    public void RemoveAllActiveList()
    {
        for (int i = activeItems.Count; i > 0; i--)
        {
            Item item = activeItems[i - 1];
            PoolManager.Instance.ReturnToPool(item.name, item);
            activeItems.Remove(item);
        }
        for (int i = activeMovingObjects.Count; i > 0; i--)
        {
            MovingObject movingObject = activeMovingObjects[i - 1];
            PoolManager.Instance.ReturnToPool(movingObject.name, movingObject);
            activeMovingObjects.Remove(movingObject);
        }
        for (int i = activeStationaryObjects.Count; i > 0; i--)
        {
            StationaryObject stationaryObject = activeStationaryObjects[i - 1];
            PoolManager.Instance.ReturnToPool(stationaryObject.name, stationaryObject);
            activeStationaryObjects.Remove(stationaryObject);
        }
    }
}
