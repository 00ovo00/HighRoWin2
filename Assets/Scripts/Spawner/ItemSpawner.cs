using UnityEngine;

public class ItemSpawner : BaseSpawner<Item, ItemSO>
{
    protected override void OnEnable()
    {
        // select one at random from the item data list
        ItemSO itemSO = objectDataList[Random.Range(0, objectDataList.Count)];
        
        // spawn object according to probability
        if (Random.value <= itemSO.spawnProbability)
        {
            SpawnObject(itemSO);
        }
    }
    
    protected override void SpawnObject(ItemSO itemSO)
    {
        Vector3 worldSpawnPosition = GetWorldSpawnPosition();   // get the world space position to put it on
        // get the object from the pool, initialize and spawn
        Item spawnedItem = PoolManager.Instance.SpawnFromPool<Item>(itemSO.tag, worldSpawnPosition, Quaternion.identity);
        spawnedItem.Initialize(itemSO.tag, itemSO.score);
        spawnedItem.OnSpawned();
    }
}