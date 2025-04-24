using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string _poolTag;
    private int _score;
    private float _activeTime;
    
    [SerializeField] private LayerMask layerMask; // 확인할 레이어 마스크(Road로 설정)

    private void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, 1, layerMask))
        {
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(_poolTag, this);
        }
    }

    public void Initialize(string tag, int itemScore, float itemDuration)
    {
        _poolTag = tag;
        _score = itemScore;
        _activeTime = itemDuration;

        // 생성하며 일정 시간 후에는 자동으로 반환되도록 설정
        //StartCoroutine(ReturnToPoolAfterDuration());
    }

    private IEnumerator ReturnToPoolAfterDuration()
    {
        yield return new WaitForSeconds(_activeTime);
        PlaySceneManager.Instance.RemoveActiveList(this);
        PoolManager.Instance.ReturnToPool(_poolTag, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.RowCount += _score;    // 각 아이템 점수만큼 row 증가
            SoundManager.Instance.PlayItemSFX();    // 아이템 획득 효과음 재생
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(_poolTag, this);    // 아이템을 트리거한 경우에는 바로 풀로 반환
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Road") && isActiveAndEnabled)
        {
            PlaySceneManager.Instance.RemoveActiveList(this);
            PoolManager.Instance.ReturnToPool(_poolTag, this);
        }
    }
}