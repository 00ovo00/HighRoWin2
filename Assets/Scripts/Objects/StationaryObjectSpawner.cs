using UnityEngine;

public class StationaryObjectSpawner : BaseSpawner<StationaryObject, StationarySO>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        
        // 고정 오브젝트 데이터 리스트에서 랜덤으로 하나를 선택
        StationarySO stationarySO = objectDataList[Random.Range(0, objectDataList.Count)];
        
        // 확률에 따라 생성
        if (Random.value <= stationarySO.spawnProbability)
        {
            SpawnObject(stationarySO);
        }
    }
    
    private void SpawnObject(StationarySO stationarySO)
    {
        Vector3 worldSpawnPosition = GetWorldSpawnPosition();
        StationaryObject spawnedObject = PoolManager.Instance.SpawnFromPool<StationaryObject>(stationarySO.tag, worldSpawnPosition, Quaternion.identity);
        spawnedObject.Initialize(stationarySO.tag);
        spawnedObject.OnSpawned();
    }
}