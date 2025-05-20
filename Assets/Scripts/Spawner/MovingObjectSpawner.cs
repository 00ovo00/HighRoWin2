using System.Collections;
using UnityEngine;

public class MovingObjectSpawner : BaseSpawner<MovingObject, MovingSO>
{
    private Transform _spawnPoint;  // 스폰 위치
    private bool _isRight;          // 방향 확인 플래그, true면 오른쪽에서 스폰
    
    protected override void OnEnable()
    {
        _spawnPoint = transform.GetChild(0);    // 스폰 위치 가져오기
        _isRight = _spawnPoint.gameObject.name == "RightSpawnPoint";    // 스폰 위치 오브젝트 이름으로 방향 확인
        
        StartCoroutine(SpawnRoutine()); // 스폰 코루틴 실행
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 움직이는 오브젝트 데이터 리스트에서 랜덤으로 하나의 정보 가져오기
            MovingSO movingSO = objectDataList[Random.Range(0, objectDataList.Count)];
            yield return new WaitForSeconds(movingSO.spawnInterval);     // 스폰 간격만큼 대기
            
            if (Random.value <= movingSO.spawnProbability)
            {
                SpawnObject(movingSO);  // 스폰 확률에 따라 오브젝트 스폰
            }
        }
    }
    
    protected override void SpawnObject(MovingSO movingSO)
    {
        float moveDir = _isRight ? -1.0f : 1.0f;     // 이동할 방향 설정
        // 풀에서 오브젝트 가져와 초기화하고 스폰
        MovingObject movingObject = PoolManager.Instance.SpawnFromPool<MovingObject>(movingSO.tag, _spawnPoint.position, Quaternion.identity);
        movingObject.Initialize(movingSO.tag, moveDir, movingSO.speed);
        movingObject.OnSpawned();
    }
}