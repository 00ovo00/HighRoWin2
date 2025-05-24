using System.IO;
using UnityEngine;

[System.Serializable]

// save play information
public class PlayInfo
{
    public int highScore;
    public int currentCharacterIndex;
    public int currentCoin;
    public int totalCoin;
    public bool[] characterStateArr;    // array that saves the character available or not
}

public class SaveManager : SingletonBase<SaveManager>
{
    private const string SaveFileName = "PlayInfoData.json";
    private PlayInfo _playInfo = new PlayInfo();
    private int _characterNum;  // total character number

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        
        // initialize play infomation
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
        
        if (File.Exists(path))  // if file exist in save data path
        {
            // get file and deserialize data to use
            string json = File.ReadAllText(path);
            _playInfo = JsonUtility.FromJson<PlayInfo>(json);
            //Debug.Log(json);
        }
        else    // if file doesn't exist in save data path
        {
            InitializeDefaultData();    // initialize data
        }
    }

    // initialize and save to default values if there is no data
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
    public void UpdateHighScore(int score)
    {
        if (score > _playInfo.highScore)    // if a new score is higher than the current high score
        {
            _playInfo.highScore = score;    // update the high score to a new score
            SaveData();
        }
    }
    
    public int GetCurrentCoin() { return _playInfo.currentCoin; }
    public void UpdateCurrentCoin(int coin)
    {
        _playInfo.currentCoin += coin;  // add the new coins to the current coins
        SaveData();
    }
    
    public int GetTotalCoin() { return _playInfo.totalCoin; }
    public void UpdateTotalCoin(int coin)
    {
        _playInfo.totalCoin += coin;    // add the new coins to the total coins
        SaveData();
    }
    
    public void UpdateGameData(int score, int coin)
    {
        UpdateCurrentCoin(coin);
        UpdateTotalCoin(coin);
        UpdateHighScore(score);
    }

    public int GetCurCharacterIdx() { return _playInfo.currentCharacterIndex; }
    public void UpdateCurCharacterIdx(int idx)
    {
        if (_playInfo.currentCharacterIndex != idx) // if the new character index is different from the current character index
        {
            _playInfo.currentCharacterIndex = idx;  // update the current character index to the new character index
            SaveData();
        }
    }
    
    // check the character is available
    public bool IsCharacterAvailable (int idx) { return _playInfo.characterStateArr[idx]; }
    public void UpdateCharacterState(int idx)
    {
        _playInfo.characterStateArr[idx] = true;    // update the character matching the index to enabled
        SaveData();
    }
}