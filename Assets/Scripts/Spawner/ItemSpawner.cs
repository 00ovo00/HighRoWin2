using UnityEngine;

public class ItemSpawner : BaseSpawner<Item, ItemSO>
{
    protected override void OnEnable()
    {
        // 아이템 데이터 리스트에서 랜덤으로 하나를 선택
        ItemSO itemSO = objectDataList[Random.Range(0, objectDataList.Count)];
        
        // 확률에 따라 생성
        if (Random.value <= itemSO.spawnProbability)
        {
            SpawnObject(itemSO);
        }
    }
    
    protected override void SpawnObject(ItemSO itemSO)
    {
        Vector3 worldSpawnPosition = GetWorldSpawnPosition();   // 배치할 월드 좌표 구하기
        // 풀에서 오브젝트 가져와 초기화하고 스폰
        Item spawnedItem = PoolManager.Instance.SpawnFromPool<Item>(itemSO.tag, worldSpawnPosition, Quaternion.identity);
        spawnedItem.Initialize(itemSO.tag, itemSO.score);
        spawnedItem.OnSpawned();
    }
}