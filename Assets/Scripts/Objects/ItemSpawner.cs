using UnityEngine;

public class ItemSpawner : BaseSpawner<Item, ItemSO>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        
        // 아이템 데이터 리스트에서 랜덤으로 하나를 선택
        ItemSO itemSO = objectDataList[Random.Range(0, objectDataList.Count)];
        
        // 확률에 따라 생성
        if (Random.value <= itemSO.spawnProbability)
        {
            SpawnItem(itemSO);
        }
    }
    
    private void SpawnItem(ItemSO itemSO)
    {
        Vector3 worldSpawnPosition = GetWorldSpawnPosition();
        Item spawnedItem = PoolManager.Instance.SpawnFromPool<Item>(itemSO.tag, worldSpawnPosition, Quaternion.identity);
        spawnedItem.Initialize(itemSO.tag, itemSO.score);
        spawnedItem.OnSpawned();
    }
}