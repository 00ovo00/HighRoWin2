using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class CharacterManager : SingletonBase<CharacterManager>
{
    public int curCharacterIdx;
    private const string CharacterDataPath = "SO";
    private GameObject _curCharacter;
    [SerializeField] GameObject characterSet;
    
    [SerializeField] private GameObject[] characterObjArr;
    [SerializeField] private CharacterSO[] characterSOArr;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        
        characterSOArr = Resources.LoadAll<CharacterSO>(CharacterDataPath);
        System.Array.Sort(characterSOArr, (a, b) => a.idx.CompareTo(b.idx));
        characterObjArr = new GameObject[characterSOArr.Length];
        
        for (int i = 0; i < characterSOArr.Length; i++)
        {
            characterObjArr[i] = Instantiate(characterSOArr[i].characterPrefab);
            characterObjArr[i].gameObject.SetActive(false);
            characterObjArr[i].transform.SetParent(characterSet.transform);
        }
    }

    public void SetCharacterObj(Transform playerTransform)
    {
        curCharacterIdx = SaveManager.Instance.GetCurCharacterIdx();
        
        _curCharacter = characterObjArr[curCharacterIdx];
        _curCharacter.transform.SetParent(playerTransform);
        _curCharacter.transform.localPosition = Vector3.zero;
        _curCharacter.transform.rotation = Quaternion.identity;
        _curCharacter.gameObject.SetActive(true);
    }

    public void ReSetCharacterObj()
    {
        _curCharacter.transform.SetParent(characterSet.transform);
        _curCharacter.gameObject.SetActive(false);
    }

    public void ChangeToPreviousCharacter()
    {
        curCharacterIdx = (curCharacterIdx - 1 + characterSOArr.Length) % characterSOArr.Length;
        SaveManager.Instance.UpdateCurCharacterIdx(curCharacterIdx);
    }
    
    public void ChangeToNextCharacter()
    {
        curCharacterIdx = (curCharacterIdx + 1) % characterSOArr.Length;
        SaveManager.Instance.UpdateCurCharacterIdx(curCharacterIdx);
    }

    public CharacterSO GetCharacterData(int index)
    {
        if (index >= 0 && index < characterSOArr.Length)
            return characterSOArr[index];
        return null;
    }

}