using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string tag;
    public int score;
    public float spawnProbability;
}