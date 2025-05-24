using UnityEngine;

[CreateAssetMenu(fileName = "MovingObjectData", menuName = "MovingSO")]
public class MovingSO : ScriptableObject
{
    public string tag;
    public float speed;
    public float spawnProbability;
    public float spawnInterval;
}