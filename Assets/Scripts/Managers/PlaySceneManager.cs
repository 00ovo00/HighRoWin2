using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : SingletonBase<PlaySceneManager>
{
    private const string PlaySceneName = "PlayScene";
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float deleteRadius;    // 시작 시 플레이어 중심으로 오브젝트 삭제할 범위

    // 활성화되어 있는 오브젝트 추적하는 리스트
    public List<Item> activeItems = new List<Item>();
    public List<MovingObject> activeMovingObjects = new List<MovingObject>();
    public List<StationaryObject> activeStationaryObjects = new List<StationaryObject>();
    
    protected override void Awake()
    {
        // 현재 활성화된 씬이 플레이씬이 아니면 삭제
        if (SceneManager.GetActiveScene().name != PlaySceneName)
        {
            Destroy(gameObject);
            return;
        }
        
        base.Awake();

        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        deleteRadius = 3;
    }

    private void Start()
    {
        CharacterManager.Instance.SetCharacterObj(playerTransform); // 플레이어를 초기 상태로 세팅
        DeleteAroundObject();   // 플레이어 주변 오브젝트 삭제
    }

    // 현재 활성화되어 있는 모든 오브젝트를 풀로 반환하고 리스트 비우기
    public void RemoveAllActiveList()
    {
        for (int i = activeItems.Count; i > 0; i--)
        {
            Item item = activeItems[i - 1];
            activeItems.Remove(item);
            Destroy(item.gameObject);
        }
        for (int i = activeMovingObjects.Count; i > 0; i--)
        {
            MovingObject movingObject = activeMovingObjects[i - 1];
            activeMovingObjects.Remove(movingObject);
            Destroy(movingObject.gameObject);
        }
        for (int i = activeStationaryObjects.Count; i > 0; i--)
        {
            StationaryObject stationaryObject = activeStationaryObjects[i - 1];
            activeStationaryObjects.Remove(stationaryObject);
            Destroy(stationaryObject.gameObject);
        }
    }

    // 초기 시작 시 플레이어 주변으로 오브젝트 배치하지 않도록 지우기
    private void DeleteAroundObject()
    {
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, deleteRadius);
        foreach (Collider col in colliders)
        {
            // road 오브젝트이면 넘기기
            if (col.gameObject.layer == LayerMask.NameToLayer("Road")) continue;
            
            // StationaryObject면 반환
            StationaryObject stationaryObject = col.GetComponent<StationaryObject>();
            if (stationaryObject != null)
            {
                stationaryObject.ReturnToPool();
                continue;
            }
            
            // Item이면 반환
            Item item = col.GetComponent<Item>();
            if (item != null)
            {
                item.ReturnToPool();
            }
        }
    }
}
