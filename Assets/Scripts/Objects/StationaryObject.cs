using UnityEngine;

public class StationaryObject : PoolableObject
{
    public void Initialize(string tag)
    {
        this.poolTag = tag;
    }
    
    // private void Update()
    // {
    //     Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
    //     RaycastHit hit;
    //
    //     if (!Physics.Raycast(ray, out hit, 1, layerMask))
    //     {
    //         PlaySceneManager.Instance.RemoveActiveList(this);
    //         PoolManager.Instance.ReturnToPool(name, this);
    //     }
    // }
}