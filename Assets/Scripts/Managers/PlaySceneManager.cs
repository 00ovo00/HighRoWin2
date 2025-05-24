using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : SingletonBase<PlaySceneManager>
{
    private const string PlaySceneName = "PlayScene";
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float deleteRadius;    // range to delete objects around the player when starting

    [SerializeField] private PoolManager.PoolConfig[] poolConfigs;  // array that sets pooling objects
    
    // list that chases activated objects
    public List<Item> activeItems = new List<Item>();
    public List<MovingObject> activeMovingObjects = new List<MovingObject>();
    public List<StationaryObject> activeStationaryObjects = new List<StationaryObject>();
    
    protected override void Awake()
    {
        // delete this if it is not the play scene
        if (SceneManager.GetActiveScene().name != PlaySceneName)
        {
            Destroy(gameObject);
            return;
        }
        
        base.Awake();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // add pools based on information of pool settings
        PoolManager.Instance.AddPools<Item>(poolConfigs);
        PoolManager.Instance.AddPools<MovingObject>(poolConfigs);
        PoolManager.Instance.AddPools<StationaryObject>(poolConfigs);
    }

    private void Start()
    {
        CharacterManager.Instance.SetCharacterObj(playerTransform); // initialize player
        DeleteAroundObject();   // delete objects around player
    }

    // return all active objects and empty list
    public void RemoveAllActiveList()
    {
        for (int i = activeItems.Count; i > 0; i--)
        {
            Item item = activeItems[i - 1];
            PoolManager.Instance.ReturnToPool(item.name, item);
            activeItems.Remove(item);
        }
        for (int i = activeMovingObjects.Count; i > 0; i--)
        {
            MovingObject movingObject = activeMovingObjects[i - 1];
            PoolManager.Instance.ReturnToPool(movingObject.name, movingObject);
            activeMovingObjects.Remove(movingObject);
        }
        for (int i = activeStationaryObjects.Count; i > 0; i--)
        {
            StationaryObject stationaryObject = activeStationaryObjects[i - 1];
            PoolManager.Instance.ReturnToPool(stationaryObject.name, stationaryObject);
            activeStationaryObjects.Remove(stationaryObject);
        }
    }

    // delete object around the player when starting
    private void DeleteAroundObject()
    {
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, deleteRadius);
        foreach (var collider in colliders)
        {
            // pass if it is the road object
            if (collider.gameObject.layer == LayerMask.NameToLayer("Road")) continue;
            
            StationaryObject stationaryObject = collider.GetComponent<StationaryObject>();
            if (stationaryObject != null)
            {
                stationaryObject.ReturnToPool();
                continue;
            }
            
            Item item = collider.GetComponent<Item>();
            if (item != null)
            {
                item.ReturnToPool();
                continue;
            }
        }
    }
}
