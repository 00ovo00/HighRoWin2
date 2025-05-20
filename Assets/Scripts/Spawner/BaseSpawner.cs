using System.Collections.Generic;
using UnityEngine;

// Poolable한 오브젝트만 스폰 대상으로 하며, 스폰 시 필요한 SO 데이터를 포함하도록 설정
public abstract class BaseSpawner<T, TSO> : MonoBehaviour
    where T : PoolableObject
    where TSO : ScriptableObject
{
    [SerializeField] protected List<TSO> objectDataList;    // Inspector에서 스폰할 대상 SO를 선택
    
    protected virtual void OnEnable()
    {
    }
    
    // 생성할 월드 좌표를 구하기
    protected Vector3 GetWorldSpawnPosition()
    {
        Vector3 randLocalPos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        return transform.TransformPoint(randLocalPos);  // -0.1f ~ 0.1f 사이의 랜덤으로 생성된 로컬 좌표를 월드 좌표로 변환
    }

    protected virtual void SpawnObject(TSO so)
    {
        // 스폰 로직을 자식 클래스에서 구현
    }
}