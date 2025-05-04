using UnityEngine;

public class Item : PoolableObject
{
    [SerializeField] private int score;
    
    public void Initialize(string tag, int score)
    {
        this.poolTag = tag;
        this.score = score;
    }

    public override void OnSpawned()
    {
        PlaySceneManager.Instance.activeItems.Add(this);
    }
    
    public override void OnDespawned()
    {
        PlaySceneManager.Instance.activeItems.Remove(this);
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        PoolManager.Instance.ReturnToPool(poolTag, this);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.RowCount += score;    // 각 아이템 점수만큼 row 증가
            SoundManager.Instance.PlayItemSFX();       // 아이템 획득 효과음 재생
            ReturnToPool();                            // 아이템을 트리거한 경우에는 바로 풀로 반환
        }
    }
    
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Road") && isActiveAndEnabled)
    //     {
    //         ReturnToPool();
    //     }
    // }
}