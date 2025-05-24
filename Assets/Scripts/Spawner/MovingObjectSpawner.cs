using System.Collections;
using UnityEngine;

public class MovingObjectSpawner : BaseSpawner<MovingObject, MovingSO>
{
    private Transform _spawnPoint;
    private bool _isRight;  // check direction, if it is true, spawned on the right
    
    protected override void OnEnable()
    {
        _spawnPoint = transform.GetChild(0);
        _isRight = _spawnPoint.gameObject.name == "RightSpawnPoint";    // check the direction by its name
        
        StartCoroutine(SpawnRoutine());
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // select one at random from the moving object data list
            MovingSO movingSO = objectDataList[Random.Range(0, objectDataList.Count)];
            yield return new WaitForSeconds(movingSO.spawnInterval);     // wait for spawn interval
            
            // spawn object according to probability
            if (Random.value <= movingSO.spawnProbability)
            {
                SpawnObject(movingSO);
            }
        }
    }
    
    protected override void SpawnObject(MovingSO movingSO)
    {
        float moveDir = _isRight ? -1.0f : 1.0f;     // set direction to move
        // get the object from the pool, initialize and spawn
        MovingObject movingObject = PoolManager.Instance.SpawnFromPool<MovingObject>(movingSO.tag, _spawnPoint.position, Quaternion.identity);
        movingObject.Initialize(movingSO.tag, moveDir, movingSO.speed);
        movingObject.OnSpawned();
    }
}