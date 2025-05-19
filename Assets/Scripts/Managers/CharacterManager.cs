using UnityEngine;

public class CharacterManager : SingletonBase<CharacterManager>
{
    public int curCharacterIdx; // 현재 플레이하고 있는 캐릭터의 인덱스
    private const string CharacterDataPath = "SO";  // Resources 폴더 내 ScriptableObject 데이터 파일 폴더명
    private GameObject _curCharacter;
    [SerializeField] GameObject characterSet;   // 생성된 캐릭터를 하나로 관리할 부모 오브젝트
    
    [SerializeField] private GameObject[] characterObjArr;  // 생성된 캐릭터 오브젝트 배열
    [SerializeField] private CharacterSO[] characterSOArr;  // 캐릭터 데이터를 저장하는 배열
    // characterSOArr 정보 기반으로 characterObjArr의 오브젝트 배열을 생성
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        
        // Resources/SO 폴더에서 모든 캐릭터 정보 받아오고 인덱스 순으로 정렬하여 배열 생성
        characterSOArr = Resources.LoadAll<CharacterSO>(CharacterDataPath);
        System.Array.Sort(characterSOArr, (a, b) => a.idx.CompareTo(b.idx));
        characterObjArr = new GameObject[characterSOArr.Length];
        
        for (int i = 0; i < characterSOArr.Length; i++)
        {
            // 프리팹으로 캐릭터 인스턴스 생성하고 캐릭터 오브젝트 배열에 할당
            characterObjArr[i] = Instantiate(characterSOArr[i].characterPrefab);
            // 비활성화하고 캐릭터Set 부모 오브젝트의 자식으로 설정
            characterObjArr[i].gameObject.SetActive(false);
            characterObjArr[i].transform.SetParent(characterSet.transform);
        }
    }

    // 플레이어 캐릭터 초기 세팅
    public void SetCharacterObj(Transform playerTransform)
    {
        curCharacterIdx = SaveManager.Instance.GetCurCharacterIdx();    // 현재 사용하는 캐릭터 인덱스 값 받아오기
        
        _curCharacter = characterObjArr[curCharacterIdx];   // 현재 캐릭터를 캐릭터 오브젝트 배열에서 찾아 설정
        _curCharacter.transform.SetParent(playerTransform); // 플레이어 오브젝트의 자식으로 설정
        
        // transform을 기본으로 설정하고 활성화
        _curCharacter.transform.localPosition = Vector3.zero;
        _curCharacter.transform.rotation = Quaternion.identity;
        _curCharacter.gameObject.SetActive(true);
    }

    // 캐릭터 오브젝트를 플레이어의 자식에서 다시 캐릭터 Set의 자식으로 설정
    public void ReSetCharacterObj()
    {
        _curCharacter.transform.SetParent(characterSet.transform);  // 캐릭터 Set의 자식으로 설정 
        _curCharacter.gameObject.SetActive(false);
    }

    // 현재 캐릭터 인덱스 배열을 이전으로 변경
    public void ChangeToPreviousCharacter()
    {
        curCharacterIdx = (curCharacterIdx - 1 + characterSOArr.Length) % characterSOArr.Length;
    }
    
    // 현재 캐릭터 인덱스 배열을 다음으로 변경
    public void ChangeToNextCharacter()
    {
        curCharacterIdx = (curCharacterIdx + 1) % characterSOArr.Length;
    }

    // 캐릭터 데이터 반환
    public CharacterSO GetCharacterData(int index)
    {
        if (index >= 0 && index < characterSOArr.Length)
            return characterSOArr[index];
        return null;
    }

    // 캐릭터 구입 시 비용 처리와 상태 갱신
    public void BuyCharacter()
    {
        // 이미 구매한 상태면 바로 리턴
        if (SaveManager.Instance.IsCharacterAvailable(curCharacterIdx)) return;
        
        // 구입 후 남은 비용 계산
        int newCoin = SaveManager.Instance.GetCurrentCoin() - GetCharacterData(curCharacterIdx).requiredSweet;
        if (newCoin < 0) return;    // 남은 비용이 음수면 구매 불가
        
        // 남은 비용이 양수면 비용 처리하고 캐릭터 이용 가능 상태로 갱신
        SaveManager.Instance.UpdateCurrentCoin(-GetCharacterData(curCharacterIdx).requiredSweet);
        SaveManager.Instance.UpdateCharacterState(curCharacterIdx);
    }
}