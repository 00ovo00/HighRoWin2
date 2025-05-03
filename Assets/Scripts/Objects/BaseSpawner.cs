using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawner<T, TSO> : MonoBehaviour
    where T : PoolableObject
    where TSO : ScriptableObject
{
    [SerializeField] protected List<TSO> objectDataList;
    
    protected virtual void OnEnable()
    {
        // 스폰 로직을 자식 클래스에서 구현
    }
    
    protected Vector3 GetWorldSpawnPosition()
    {
        Vector3 randLocalPos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        return transform.TransformPoint(randLocalPos);  // -0.1f ~ 0.1f 사이의 랜덤으로 생성된 로컬 좌표를 월드 좌표로 변환
    }
}