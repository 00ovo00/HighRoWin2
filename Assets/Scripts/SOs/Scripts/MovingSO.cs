using UnityEngine;

[CreateAssetMenu(fileName = "MovingObjectData", menuName = "MovingSO")]
public class MovingSO : ScriptableObject
{
    public string tag;                   // 풀에서 사용할 태그
    public float speed = 5.0f;           // 이동 속도
    public float spawnProbability = 0.7f; // 스폰 확률
    public float spawnInterval = 5.0f;   // 스폰 간격
}