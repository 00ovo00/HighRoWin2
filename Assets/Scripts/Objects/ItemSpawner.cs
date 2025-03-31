using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<ItemSO> itemSOList;

    private void OnEnable()
    {
        // 아이템 정보 리스트에서 랜덤으로 하나를 선택하여 확률에 따라 생성
        ItemSO itemSo = itemSOList[Random.Range(0, itemSOList.Count)];

        // 가져온 아이템 정보 기반하여 일정 확률로 아이템 생성
        if (Random.value <= itemSo.spawnProbability)
        {
            SpawnItem(itemSo);
        }
    }

    private void SpawnItem(ItemSO itemSO)
    {
        // -0.1f ~ 0.1f 사이의 랜덤으로 생성된 로컬 좌표를 월드 좌표로 변환
        Vector3 localSpawnPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);

        // 생성 시에는 월드 좌표 기준으로 풀에서 가져오기
        GameObject spawnedItem = PoolingManager.Instance.SpawnFromPool(itemSO.tag, worldSpawnPosition, Quaternion.identity);

        // 생성된 아이템에 있는 스크립트 정보 가져오기
        Item itemScript = spawnedItem.GetComponent<Item>();
        if (itemScript == null)
        {
            itemScript = spawnedItem.AddComponent<Item>();
        }
        // 가져온 정보를 토대로 초기화
        itemScript.Initialize(itemSO.tag, itemSO.score, itemSO.activeTime);
    }
}