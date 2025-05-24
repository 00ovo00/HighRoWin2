using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int idx;
    public string characterName;
    public int requiredSweet;   // need to unlock character
    public GameObject characterPrefab;
}
