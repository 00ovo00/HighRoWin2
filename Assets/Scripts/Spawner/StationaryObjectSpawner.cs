using UnityEngine;

public class StationaryObjectSpawner : BaseSpawner<StationaryObject, StationarySO>
{
    protected override void OnEnable()
    {
        // select one at random from the stationary object data list
        StationarySO stationarySO = objectDataList[Random.Range(0, objectDataList.Count)];
        
        // spawn object according to probability
        if (Random.value <= stationarySO.spawnProbability)
        {
            SpawnObject(stationarySO);
        }
    }
    
    protected override void SpawnObject(StationarySO stationarySO)
    {
        Vector3 worldSpawnPosition = GetWorldSpawnPosition();   // get the world space position to put it on
        // get the object from the pool, initialize and spawn
        StationaryObject spawnedObject = PoolManager.Instance.SpawnFromPool<StationaryObject>(stationarySO.tag, worldSpawnPosition, Quaternion.identity);
        spawnedObject.Initialize(stationarySO.tag);
        spawnedObject.OnSpawned();
    }
}