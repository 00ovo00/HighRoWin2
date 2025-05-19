using System.IO;
using UnityEngine;

[System.Serializable]

// 플레이 정보를 저장하는 클래스
public class PlayInfo
{
    public int highScore;   // 최고점
    public int currentCharacterIndex;   // 현재 플레이하는 캐릭터의 인덱스
    public int currentCoin; // 현재 보유 재화
    public int totalCoin;   // 총 누적 재화
    public bool[] characterStateArr;    // 캐릭터 가용 상태 저장하는 배열
}

public class SaveManager : SingletonBase<SaveManager>
{
    private const string SaveFileName = "PlayInfoData.json";    // 플레이 데이터 저장 파일명
    private PlayInfo _playInfo = new PlayInfo();
    private int _characterNum;  // 총 캐릭터 종류 개수

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        
        // 플레이 정보 초기화
        _playInfo.highScore = -1;
        _playInfo.currentCharacterIndex = -1;
        _playInfo.currentCoin = -1;
        _playInfo.totalCoin = -1;
        _characterNum = 7;
        _playInfo.characterStateArr = new bool[_characterNum];
    }

    private void OnEnable()
    {
        LoadData();
    }

    // 저장 경로 가져오기
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(_playInfo);
        File.WriteAllText(GetSavePath(), json);
    }

    private void LoadData()
    {
        string path = GetSavePath();
        //Debug.Log(path);
        
        if (File.Exists(path))  // 저장 경로에 파일이 있으면
        {
            string json = File.ReadAllText(path);   // json 형태로 파일 읽기
            _playInfo = JsonUtility.FromJson<PlayInfo>(json);   // 역직렬화하여 플레이 정보 가져오기
            //Debug.Log(json);
        }
        else    // 저장 경로에 파일 없으면
        {
            InitializeDefaultData();    // 데이터 초기화
        }
    }

    // 데이터 없는 경우 기본 값으로 초기화하고 저장
    private void InitializeDefaultData()
    {
        _playInfo.highScore = 0;
        _playInfo.currentCharacterIndex = 0;
        _playInfo.currentCoin = 0;
        _playInfo.totalCoin = 0;
        _playInfo.characterStateArr[0] = true;
        for (int i = 1; i < _characterNum - 1; i++)
        {
            _playInfo.characterStateArr[i] = false;
        }
        SaveData();
    }

    public int GetHighscore() { return _playInfo.highScore; }
    // 최고점 갱신
    public void UpdateHighScore(int score)
    {
        if (score > _playInfo.highScore)    // 새로운 점수가 기존 최고점보다 높으면
        {
            _playInfo.highScore = score;    // 최고점을 새 점수로 갱신
            SaveData();
        }
    }
    
    public int GetCurrentCoin() { return _playInfo.currentCoin; }
    // 현재 보유 재화 갱신
    public void UpdateCurrentCoin(int coin)
    {
        _playInfo.currentCoin += coin;  // 현재 보유 재화에 새로 얻은 코인 누적
        SaveData();
    }
    
    public int GetTotalCoin() { return _playInfo.totalCoin; }
    // 총 획득 재화 갱신
    public void UpdateTotalCoin(int coin)
    {
        _playInfo.totalCoin += coin;    // 현재 총 획득 재화에 새로 얻은 코인 누적
        SaveData();
    }
    
    public void UpdateGameData(int score, int coin)
    {
        UpdateCurrentCoin(coin);
        UpdateTotalCoin(coin);
        UpdateHighScore(score);
    }

    public int GetCurCharacterIdx() { return _playInfo.currentCharacterIndex; }
    // 캐릭터 인덱스 값 갱신
    public void UpdateCurCharacterIdx(int idx)
    {
        if (_playInfo.currentCharacterIndex != idx) // 현재 캐릭터 인덱스 값과 새 캐릭터 인덱스 값이 다르면
        {
            _playInfo.currentCharacterIndex = idx;  // 새 인덱스 값을 현재 인덱스 값으로 변경
            SaveData();
        }
    }
    
    // 특정 인덱스의 캐릭터가 사용 가능한 상태인지 확인
    public bool IsCharacterAvailable (int idx) { return _playInfo.characterStateArr[idx]; }
    // 캐릭터 상태 갱신
    public void UpdateCharacterState(int idx)
    {
        _playInfo.characterStateArr[idx] = true;    // 인덱스와 일치하는 캐릭터를 사용 가능 상태로 변경
        SaveData();
    }
}