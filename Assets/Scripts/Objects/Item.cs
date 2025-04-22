using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string _poolTag;
    private int _score;
    private float _activeTime;

    public void Initialize(string tag, int itemScore, float itemDuration)
    {
        _poolTag = tag;
        _score = itemScore;
        _activeTime = itemDuration;

        // 생성하며 일정 시간 후에는 자동으로 반환되도록 설정
        StartCoroutine(ReturnToPoolAfterDuration());
    }

    private IEnumerator ReturnToPoolAfterDuration()
    {
        yield return new WaitForSeconds(_activeTime);
        PoolingManager.Instance.ReturnToPool(_poolTag, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.RowCount += _score;    // 각 아이템 점수만큼 row 증가
            SoundManager.Instance.PlayItemSFX();    // 아이템 획득 효과음 재생
            PoolingManager.Instance.ReturnToPool(_poolTag, gameObject);    // 아이템을 트리거한 경우에는 바로 풀로 반환
        }
    }
}