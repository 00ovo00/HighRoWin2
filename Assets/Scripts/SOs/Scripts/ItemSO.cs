using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string tag;                   // 풀에서 사용할 태그
    public int score;                    // 점수
    public float spawnProbability;       // 스폰 확률
    public float activeTime;             // 활성화 되어 있는 시간
}