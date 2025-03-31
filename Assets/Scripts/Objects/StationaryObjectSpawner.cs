using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationaryObject
{
    public string tag;                  // 풀 식별 태그
    public float spawnProbability;      // 스폰 확률
    public float activeTime;            // 활성화되어 있는 시간
}

public class StationaryObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<StationaryObject> stationaryObjectList;

    private void OnEnable()
    {
        // 움직이지 않는 오브젝트 리스트에서 랜덤으로 하나를 선택하여 확률에 따라 생성
        StationaryObject stationaryObject = stationaryObjectList[Random.Range(0, stationaryObjectList.Count)];

        if (Random.value <= stationaryObject.spawnProbability)
        {
            SpawnObject(stationaryObject);
        }
    }
    
    private void SpawnObject(StationaryObject stationaryObject)
    {
        // -0.1f ~ 0.1f 사이의 랜덤으로 생성된 로컬 좌표를 월드 좌표로 변환
        Vector3 localSpawnPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);

        // 생성 시에는 월드 좌표 기준으로 풀에서 가져오기
        GameObject spawnedObject = PoolingManager.Instance.SpawnFromPool(stationaryObject.tag, worldSpawnPosition, Quaternion.identity);

        // 활성 시간 이후에는 풀에 자동으로 반환되는 코루틴 실행
        StartCoroutine(ReturnToPoolAfterDelay(spawnedObject, stationaryObject.activeTime, stationaryObject.tag));
    }
    
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, string poolTag)
    {
        yield return new WaitForSeconds(delay);
        PoolingManager.Instance.ReturnToPool(poolTag, obj);
    }
}