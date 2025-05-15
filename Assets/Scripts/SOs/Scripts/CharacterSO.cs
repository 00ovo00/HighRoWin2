using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int idx;
    public string characterName;
    public int requiredSweet;
    public GameObject characterPrefab;
}
