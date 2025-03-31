using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<MovingSO> movingObjectDatas; // 생성할 풀의 이름 리스트
    private Transform _spawnPoint;    // 스폰 위치
    private bool _isRight;           // 방향 확인 플래그, true면 오른쪽에서 스폰

    private void OnEnable()
    {
        _spawnPoint = transform.GetChild(0);
        
        // 스폰 위치 오브젝트 이름으로 방향 확인
        _isRight = _spawnPoint.gameObject.name == "RightSpawnPoint";

        // 스폰 코루틴 실행
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 움직이는 오브젝트 데이터 리스트에서 랜덤으로 하나의 정보 가져오기
            MovingSO movingSO = movingObjectDatas[Random.Range(0, movingObjectDatas.Count)];
            yield return new WaitForSeconds(movingSO.spawnInterval);

            if (Random.value <= movingSO.spawnProbability)
            {
                SpawnObject(movingSO);
            }
        }
    }

    private void SpawnObject(MovingSO movingSO)
    {
        GameObject spawnedObject = PoolingManager.Instance.SpawnFromPool(movingSO.tag, _spawnPoint.position, Quaternion.identity);

        // 이동할 방향 설정
        float moveDir = _isRight ? -1.0f : 1.0f;
        
        MovingObject movingObject = spawnedObject.GetComponent<MovingObject>();
        movingObject.Initialize(moveDir, movingSO.speed);
        
        StartCoroutine(ReturnToPoolAfterDelay(spawnedObject, 10.0f, movingSO.tag));
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, string poolTag)
    {
        yield return new WaitForSeconds(delay);
        PoolingManager.Instance.ReturnToPool(poolTag, obj);
    }
}