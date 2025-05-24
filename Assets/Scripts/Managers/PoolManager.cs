using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : SingletonBase<PoolManager>
{
    // manage all pools on the inspector view at once
    [Serializable]
    public class PoolConfig
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    private List<PoolConfig> _poolConfigs = new List<PoolConfig>(); // list that manages pool setting
    private Dictionary<string, object> _pools = new Dictionary<string, object>();   // dictionary that manages multiple pool at once

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    // create new pool
    private void CreatePool<T>(string tag, GameObject prefab, int size) where T : Component
    {
        // return if pool with tag already exists (prevent creating duplicated pool)
        if (_pools.ContainsKey(tag))
        {
            //Debug.Log($"Pool with tag {tag} already exists.");
            return;
        }
        
        // organize hierarchy
        GameObject poolObject = new GameObject($"Pool_{tag}"); // create an empty game object to manage the pool and distinguish the name by tag
        poolObject.transform.SetParent(transform); // make the pool as a child of the pool manager


        // create a new object pool based on settings received from the inspector view
        IObjectPool<T> objectPool = new ObjectPool<T>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.name = tag; // set the name of the created pooling object to be the same as the tag name
                obj.transform.SetParent(poolObject.transform);
                return obj.GetComponent<T>();
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Destroy(obj.gameObject),
            defaultCapacity: size,
            maxSize: 100
        );

        ExpandPool(objectPool, size);

        _pools.Add(tag, objectPool);    // add the new object pool to the pool dictionary
    }

    // expand pool as much as size
    private void ExpandPool<T>(IObjectPool<T> pool, int size) where T : Component
    {
        Stack<T> temp = new Stack<T>();
        for (int i = 0; i < size; i++)
        {
            temp.Push(pool.Get());
        }
        for (int i = 0; i < size; i++)
        {
            pool.Release(temp.Pop());
        }
    }

    // add the new setting information to the pool setting list
    public void AddPools<T>(PoolConfig[] newPools) where T : Component
    {
        if (newPools == null) return;

        foreach (var pool in newPools)
        {
            if (_pools.ContainsKey(pool.tag)) continue; // pass if it has been already
            _poolConfigs.Add(pool); // add the pool information to the list from other classes
        }
    }

    // return a T type object from the pool (set transform version)
    public T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
    {
        // return the object from the pool after setting transform
        T obj = SpawnFromPool<T>(tag);
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        //Debug.Log($"Type Error: Pool with tag {tag} is {typeof(T)}.");
        return null;
    }

    // return a T type object from the pool
    public T SpawnFromPool<T>(string tag) where T : Component
    {
        // create a pool if there is no pool with the tag
        if (!_pools.TryGetValue(tag, out var pool))
        {
            foreach (var poolConfig in _poolConfigs)
            {
                if (poolConfig.tag == tag)
                {
                    CreatePool<T>(poolConfig.tag, poolConfig.prefab, poolConfig.size);
                }
            }
        }

        // if pool creation fails, output error message and return null
        if (!_pools.TryGetValue(tag, out pool))
        {
            //Debug.Log($"Pool with tag {tag} cannot be created.");
            return null;
        }

        // if pool with tag exits
        if (pool is IObjectPool<T> typedPool)
        {
            // if all object in the pool unavailable, expand the pool
            if (typedPool.CountInactive == 0)
            {
                var poolConfig = _poolConfigs.Find(config => config.tag == tag);
                if (poolConfig != null)
                {
                    ExpandPool(typedPool, poolConfig.size);
                }
            }

            // return the object from the pool
            T obj = typedPool.Get();
            return obj;
        }

        //Debug.Log($"Type Error: Pool with tag {tag} is {typeof(T)}.");
        return null;
    }

    public void ReturnToPool<T>(string tag, T obj) where T : Component
    {
        if (obj == null) return;

        // check if there is a pool with the tag
        if (!_pools.TryGetValue(tag, out var pool))
        {
            //Debug.Log($"Pool with tag {tag} does not exist.");
            return;
        }

        // return the object to the pool
        if (pool is IObjectPool<T> typedPool)
        {
            typedPool.Release(obj);
            return;
        }

        //Debug.Log($"Type Error: Pool with tag {tag} is {typeof(T)}.");
    }
}