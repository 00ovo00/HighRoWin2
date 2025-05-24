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
        if (other.CompareTag("Player")) // if trigger with player
        {
            DataManager.Instance.SweetCount += score;  // increase sweet by each item score
            SoundManager.Instance.PlayItemSFX();       // play SFX gets item
            ReturnToPool();                            // return to pool immediately
        }
    }
}