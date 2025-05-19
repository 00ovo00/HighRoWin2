using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : SingletonBase<PlaySceneManager>
{
    private const string PlaySceneName = "PlayScene";
    [SerializeField] private GameObject player;
    [SerializeField] private float deleteRadius;

    [SerializeField] private PoolManager.PoolConfig[] poolConfigs;
    public List<Item> activeItems = new List<Item>();
    public List<MovingObject> activeMovingObjects = new List<MovingObject>();
    public List<StationaryObject> activeStationaryObjects = new List<StationaryObject>();
    
    protected override void Awake()
    {
        if (SceneManager.GetActiveScene().name != PlaySceneName)
        {
            Destroy(gameObject);
        }
        
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player");

        PoolManager.Instance.AddPools<Item>(poolConfigs);
        PoolManager.Instance.AddPools<MovingObject>(poolConfigs);
        PoolManager.Instance.AddPools<StationaryObject>(poolConfigs);
    }

    private void Start()
    {
        CharacterManager.Instance.SetCharacterObj(player.transform);
        DeleteAroundObject();   // 플레이어 주변 오브젝트 삭제
    }

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

    // 초기 시작 시 플레이어 주변으로 오브젝트 배치하지 않도록 지우기
    private void DeleteAroundObject()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, deleteRadius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Road"))
                continue;
            
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
