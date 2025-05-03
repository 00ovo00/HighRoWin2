using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaySceneManager : SingletonBase<PlaySceneManager>
{
    [SerializeField] private PoolManager.PoolConfig[] poolConfigs;
    [SerializeField] private List<Item> activeItems = new List<Item>();
    [SerializeField] private List<MovingObject> activeMovingObjects = new List<MovingObject>();
    [SerializeField] private List<StationaryObject> activeStationaryObjects = new List<StationaryObject>();
    
    protected override void Awake()
    {
        base.Awake();
        PoolManager.Instance.AddPools<Item>(poolConfigs);
        PoolManager.Instance.AddPools<MovingObject>(poolConfigs);
        PoolManager.Instance.AddPools<StationaryObject>(poolConfigs);
    }

    public void AddActiveList(PoolableObject poolableObject)
    {
        if (poolableObject is Item item)
            activeItems.Add(item);
        else if (poolableObject is MovingObject movingObject)
            activeMovingObjects.Add(movingObject);
        else if (poolableObject is StationaryObject stationaryObject)
            activeStationaryObjects.Add(stationaryObject);
    }

    public void RemoveActiveList(PoolableObject poolableObject)
    {
        if (poolableObject is Item item)
            activeItems.Remove(item);
        else if (poolableObject is MovingObject movingObject)
            activeMovingObjects.Remove(movingObject);
        else if (poolableObject is StationaryObject stationaryObject)
            activeStationaryObjects.Remove(stationaryObject);
    }

    public void RemoveAllActiveList()
    {
        for (int i = activeItems.Count; i > 0; i--)
        {
            Item item = activeItems[i - 1];
            PoolManager.Instance.ReturnToPool(item.name, item);
            RemoveActiveList(item);
        }
        for (int i = activeMovingObjects.Count; i > 0; i--)
        {
            MovingObject movingObject = activeMovingObjects[i - 1];
            PoolManager.Instance.ReturnToPool(movingObject.name, movingObject);
            RemoveActiveList(movingObject);
        }
        for (int i = activeStationaryObjects.Count; i > 0; i--)
        {
            StationaryObject stationaryObject = activeStationaryObjects[i - 1];
            PoolManager.Instance.ReturnToPool(stationaryObject.name, stationaryObject);
            RemoveActiveList(stationaryObject);
        }
    }
}
