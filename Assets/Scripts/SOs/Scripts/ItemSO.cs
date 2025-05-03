using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string tag;                   // 풀에서 사용할 태그
    public int score;                    // 점수
    public float spawnProbability;       // 스폰 확률
}