using UnityEngine;

[CreateAssetMenu(fileName = "StationaryObjectData", menuName = "StationarySO")]
public class StationarySO : ScriptableObject
{
    public string tag;
    public float spawnProbability;
}