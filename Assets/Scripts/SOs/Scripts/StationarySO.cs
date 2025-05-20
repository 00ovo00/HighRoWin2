using UnityEngine;

[CreateAssetMenu(fileName = "StationaryObjectData", menuName = "StationarySO")]
public class StationarySO : ScriptableObject
{
    public string tag;  // 풀에서 사용할 태그
    public float spawnProbability; // 스폰 확률
}