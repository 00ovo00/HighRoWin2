using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : SingletonBase<PoolingManager>
{
    [System.Serializable]
    public class PoolConfig
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<PoolConfig> pools;  // 풀 리스트 inspector에서 확인
    private Dictionary<string, ObjectPool> _poolDictionary;

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        _poolDictionary = new Dictionary<string, ObjectPool>();

        foreach (var poolConfig in pools)
        {
            // 각 풀의 이름을 부모 오브젝트로 설정하여 계층 구조 정리
            GameObject poolObject = new GameObject($"@{poolConfig.tag}_Pool");
            poolObject.transform.SetParent(transform);

            // ObjectPool 클래스 활용하여 프리팹을 초기 개수만큼 준비
            ObjectPool objectPool = poolObject.AddComponent<ObjectPool>();
            objectPool.Initialize(poolConfig.prefab, poolConfig.size);
            
            // 생성된 풀 오브젝트를 PoolingManager의 자식으로 설정
            objectPool.transform.SetParent(poolObject.transform);

            // 반납, 재활용 시 빠르게 찾도록 딕셔너리로 등록
            _poolDictionary.Add(poolConfig.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // 풀에 해당 키 값 있는지 유효성 체크
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }
        
        // 키 검색으로 풀에서 오브젝트 가져오고 transform 설정 후 활성화
        GameObject objectToSpawn = _poolDictionary[tag].SpawnFromPool();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        // 풀에 해당 키 값 있는지 유효성 체크
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return;
        }

        // 오브젝트를 비활성화하고 각 풀의 자식 객체로 들어가도록 설정한 후 반환
        obj.SetActive(false);
        obj.transform.SetParent(_poolDictionary[tag].transform);
        _poolDictionary[tag].ReturnToPool(obj);
    }
}
