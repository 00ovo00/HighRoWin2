using UnityEngine;

public class StationaryObject : MonoBehaviour
{
    public string tag;                  // 풀 식별 태그
    public float spawnProbability;      // 스폰 확률
    public float activeTime;            // 활성화되어 있는 시간

    [SerializeField] private LayerMask layerMask; // 확인할 레이어 마스크(Road로 설정)

    private void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 1, layerMask))
        {
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(name, this);
        }
    }
}