using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlaySceneManager : SingletonBase<PlaySceneManager>
{
    [SerializeField] private PoolManager.PoolConfig[] _poolConfigs;
    [SerializeField] private List<Item> activeItems = new List<Item>();
    [SerializeField] private List<MovingObject> activeMovingObjects = new List<MovingObject>();
    [SerializeField] private List<StationaryObject> activeStationaryObjects = new List<StationaryObject>();
    
    protected override void Awake()
    {
        base.Awake();
        PoolManager.Instance.AddPools<Item>(_poolConfigs);
        PoolManager.Instance.AddPools<MovingObject>(_poolConfigs);
        PoolManager.Instance.AddPools<StationaryObject>(_poolConfigs);
    }

    public void AddActiveList(Item item) { activeItems.Add(item); }
    public void AddActiveList(MovingObject movingObject) { activeMovingObjects.Add(movingObject); }
    public void AddActiveList(StationaryObject stationaryObject) { activeStationaryObjects.Add(stationaryObject); }
    
    public void RemoveActiveList(Item item) { activeItems.Remove(item); }
    public void RemoveActiveList(MovingObject movingObject) { activeMovingObjects.Remove(movingObject); }
    public void RemoveActiveList(StationaryObject stationaryObject) { activeStationaryObjects.Remove(stationaryObject); }

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
