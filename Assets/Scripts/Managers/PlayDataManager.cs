using System.IO;
using UnityEngine;
using System;

public interface ISaveSystem
{
    void SaveData();
    void LoadData();
    void InitializeDefaultData();
}

// 플레이 정보를 저장하는 클래스
[System.Serializable]
public class PlayInfo
{
    public int highScore;   // 최고점
    public int currentCharacterIndex;   // 현재 플레이하는 캐릭터의 인덱스
    public int currentCoin; // 현재 보유 재화
    public int totalCoin;   // 총 누적 재화
    public bool[] characterStateArr;    // 캐릭터 가용 상태 저장하는 배열
}

public class PlayDataManager : SingletonBase<PlayDataManager>, ISaveSystem
{
    private const string SaveFileName = "PlayInfoData.json";
    private PlayInfo _playInfo = new PlayInfo();
    private int _characterNum = 7;
    
    // 이벤트 선언
    public static event Action<int> OnHighScoreUpdated;
    public static event Action<int> OnTotalCoinUpdated;
    public static event Action<int> OnCharacterUnlocked;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        
        // 플레이 정보 초기화
        _playInfo.highScore = -1;
        _playInfo.currentCharacterIndex = -1;
        _playInfo.currentCoin = -1;
        _playInfo.totalCoin = -1;
        _playInfo.characterStateArr = new bool[_characterNum];
    }

    private void OnEnable()
    {
        LoadData();
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(_playInfo);
        File.WriteAllText(GetSavePath(), json);
    }

    public void LoadData()
    {
        string path = GetSavePath();
        Debug.Log(path);
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _playInfo = JsonUtility.FromJson<PlayInfo>(json);
        }
        else
        {
            InitializeDefaultData();
        }
    }

    public void InitializeDefaultData()
    {
        _playInfo.highScore = 0;
        _playInfo.currentCharacterIndex = 0;
        _playInfo.currentCoin = 0;
        _playInfo.totalCoin = 0;
        _playInfo.characterStateArr[0] = true;
        for (int i = 1; i < _characterNum; i++)
        {
            _playInfo.characterStateArr[i] = false;
        }
        SaveData();
    }

    // Getter 메서드들
    public int GetHighscore() { return _playInfo.highScore; }
    public int GetCurrentCoin() { return _playInfo.currentCoin; }
    public int GetTotalCoin() { return _playInfo.totalCoin; }
    public int GetCurCharacterIdx() { return _playInfo.currentCharacterIndex; }
    public bool IsCharacterAvailable(int idx) { return _playInfo.characterStateArr[idx]; }
    public int GetUnlockedCharacterCount()
    {
        int count = 0;
        for (int i = 0; i < _playInfo.characterStateArr.Length; i++)
            if (_playInfo.characterStateArr[i]) count++;
        return count;
    }

    // Update 메서드들
    public void UpdateHighScore(int score)
    {
        if (score > _playInfo.highScore)    // 현재 점수가 기존 최고점보다 높으면
        {
            _playInfo.highScore = score;    // 최고점 변경
            OnHighScoreUpdated?.Invoke(score);  // 최고점 갱신 이벤트 호출
            SaveData();
        }
    }
    
    public void UpdateCurrentCoin(int coin)
    {
        _playInfo.currentCoin += coin;  // 현재 코인 개수 갱신
        SaveData();
    }
    
    public void UpdateTotalCoin(int coin)
    {
        _playInfo.totalCoin += coin;    // 누적 코인 개수 갱신
        OnTotalCoinUpdated?.Invoke(_playInfo.totalCoin);    // 누적 코인 갱신 이벤트 호출
        SaveData();
    }
    
    // 게임 데이터 갱신
    public void UpdateGameData(int score, int coin)
    {
        UpdateHighScore(score);     // 최고점 갱신
        UpdateCurrentCoin(coin);    // 현재 코인 갱신
        UpdateTotalCoin(coin);      // 누적 코인 갱신
    }

    public void UpdateCurCharacterIdx(int idx)
    {
        if (_playInfo.currentCharacterIndex != idx) // 현재 인덱스와 캐릭터 인덱스가 다르면
        {
            _playInfo.currentCharacterIndex = idx;  // 캐릭터 인덱스 변경
            SaveData();
        }
    }
    
    public void UpdateCharacterState(int idx)
    {
        if (!_playInfo.characterStateArr[idx])  // 현재 캐릭터 인덱스가 사용 불가 상태면
        {
            _playInfo.characterStateArr[idx] = true;    // 가용 상태로 변경
            OnCharacterUnlocked?.Invoke(GetUnlockedCharacterCount());   // 캐릭터 해금 이벤트 호출
            SaveData();
        }
    }
}