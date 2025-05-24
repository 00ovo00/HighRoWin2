using UnityEngine;

public class CharacterManager : SingletonBase<CharacterManager>
{
    public int curCharacterIdx;
    private const string CharacterDataPath = "SO";  // folder name of scriptable object file name in Resources folder
    private GameObject _curCharacter;
    [SerializeField] GameObject characterSet;   // parent object to manage instantiated characters
    
    [SerializeField] private GameObject[] characterObjArr;
    [SerializeField] private CharacterSO[] characterSOArr;
    // make characterObjArr based on characterSOArr's data
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        
        // get all character data from Resources/SO and make sorted array
        characterSOArr = Resources.LoadAll<CharacterSO>(CharacterDataPath);
        System.Array.Sort(characterSOArr, (a, b) => a.idx.CompareTo(b.idx));
        characterObjArr = new GameObject[characterSOArr.Length];
        
        for (int i = 0; i < characterSOArr.Length; i++)
        {
            // instantiate character instance by prefab and assign it to character object array
            characterObjArr[i] = Instantiate(characterSOArr[i].characterPrefab);
            // disable and make it as child of characterSet object
            characterObjArr[i].gameObject.SetActive(false);
            characterObjArr[i].transform.SetParent(characterSet.transform);
        }
    }

    // initialize player character
    public void SetCharacterObj(Transform playerTransform)
    {
        curCharacterIdx = SaveManager.Instance.GetCurCharacterIdx();
        
        _curCharacter = characterObjArr[curCharacterIdx];   // find current character by index and set character object
        _curCharacter.transform.SetParent(playerTransform); // make character object as child of player
        
        // reset transform and activate
        _curCharacter.transform.localPosition = Vector3.zero;
        _curCharacter.transform.rotation = Quaternion.identity;
        _curCharacter.gameObject.SetActive(true);
    }

    // make character object as child of character set
    public void ReSetCharacterObj()
    {
        _curCharacter.transform.SetParent(characterSet.transform); 
        _curCharacter.gameObject.SetActive(false);
    }

    // change character index array to previous
    public void ChangeToPreviousCharacter()
    {
        curCharacterIdx = (curCharacterIdx - 1 + characterSOArr.Length) % characterSOArr.Length;
    }
    
    // change character index array to next
    public void ChangeToNextCharacter()
    {
        curCharacterIdx = (curCharacterIdx + 1) % characterSOArr.Length;
    }

    // return character data
    public CharacterSO GetCharacterData(int index)
    {
        if (index >= 0 && index < characterSOArr.Length)
            return characterSOArr[index];
        return null;
    }

    // calculate costs and update character state when buying character
    public void BuyCharacter()
    {
        // return if it has been purchased already
        if (SaveManager.Instance.IsCharacterAvailable(curCharacterIdx)) return;
        
        // calculate remain after buying
        int newCoin = SaveManager.Instance.GetCurrentCoin() - GetCharacterData(curCharacterIdx).requiredSweet;
        if (newCoin < 0) return;    // if remain is minus, limit purchasing
        
        // if remain is plus, update coin and make character available
        SaveManager.Instance.UpdateCurrentCoin(-GetCharacterData(curCharacterIdx).requiredSweet);
        SaveManager.Instance.UpdateCharacterState(curCharacterIdx);
    }
}