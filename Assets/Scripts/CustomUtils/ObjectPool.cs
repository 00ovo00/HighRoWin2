using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> _objectPool;
    private GameObject _prefab;

    public void Initialize(GameObject prefab, int size)
    {
        _prefab = prefab;
        _objectPool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }

    public GameObject SpawnFromPool()
    {
        // 풀에 사용 가능한 오브젝트 있는지 확인
        if (_objectPool.Count == 0)
        {
            Debug.LogWarning("ObjectPool is empty!");
            return null;
        }

        GameObject obj = _objectPool.Dequeue();
        _objectPool.Enqueue(obj);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}