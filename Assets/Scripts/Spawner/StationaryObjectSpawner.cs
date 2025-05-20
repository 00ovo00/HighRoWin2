using UnityEngine;

public class StationaryObjectSpawner : BaseSpawner<StationaryObject, StationarySO>
{
    protected override void OnEnable()
    {
        // 고정 오브젝트 데이터 리스트에서 랜덤으로 하나를 선택
        StationarySO stationarySO = objectDataList[Random.Range(0, objectDataList.Count)];
        
        // 확률에 따라 생성
        if (Random.value <= stationarySO.spawnProbability)
        {
            SpawnObject(stationarySO);
        }
    }
    
    protected override void SpawnObject(StationarySO stationarySO)
    {
        Vector3 worldSpawnPosition = GetWorldSpawnPosition();   // 배치할 월드 좌표 구하기
        // 풀에서 오브젝트 가져와 초기화하고 스폰
        StationaryObject spawnedObject = PoolManager.Instance.SpawnFromPool<StationaryObject>(stationarySO.tag, worldSpawnPosition, Quaternion.identity);
        spawnedObject.Initialize(stationarySO.tag);
        spawnedObject.OnSpawned();
    }
}