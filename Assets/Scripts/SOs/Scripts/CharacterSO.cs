using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int idx;
    public string characterName;
    public int requiredSweet;   // 캐릭터를 해금하기 위해 필요한 재화
    public GameObject characterPrefab;  // 캐릭터 생성 시 사용할 프리팹
}
