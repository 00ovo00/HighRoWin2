using System.Collections.Generic;
using UnityEngine;

// spawn only objects that can be pooled, including SO data required for spawn
public abstract class BaseSpawner<T, TSO> : MonoBehaviour
    where T : PoolableObject
    where TSO : ScriptableObject
{
    [SerializeField] protected List<TSO> objectDataList;    // set the spawn objects on the inspector view
    
    protected virtual void OnEnable()
    {
    }
    
    // get the world space position to spawn
    protected Vector3 GetWorldSpawnPosition()
    {
        Vector3 randLocalPos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 0);
        return transform.TransformPoint(randLocalPos);  // convert randomly generated local position between -0.1f and 0.1f to world position
    }

    protected virtual void SpawnObject(TSO so)
    {
        // implement spawn logic in a child class
    }
}