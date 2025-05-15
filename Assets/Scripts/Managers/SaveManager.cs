using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayInfo
{
    public int highScore;
    public int currentCharacterIndex;
}

public class SaveManager : SingletonBase<SaveManager>
{
    private const string SaveFileName = "PlayInfoData.json";
    private PlayInfo _playInfo = new PlayInfo();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        _playInfo.highScore = -1;
        _playInfo.currentCharacterIndex = 0;
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
        Debug.Log(path);
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _playInfo = JsonUtility.FromJson<PlayInfo>(json);
            Debug.Log(json);
        }
        else
        {
            InitializeDefaultData();
        }
    }

    private void InitializeDefaultData()
    {
        _playInfo.highScore = 0;
        _playInfo.currentCharacterIndex = 0;
        SaveData();
    }

    public int GetHighscore() { return _playInfo.highScore; }
    public void UpdateHighScore(int score)
    {
        if (score > _playInfo.highScore)
        {
            _playInfo.highScore = score;
            SaveData();
        }
    }

    public int GetCurCharacterIdx() { return _playInfo.currentCharacterIndex; }
    public void UpdateCurCharacterIdx(int idx)
    {
        if (_playInfo.currentCharacterIndex != idx)
        {
            _playInfo.currentCharacterIndex = idx;
            SaveData();
        }
    }
}