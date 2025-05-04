using System.IO;
using UnityEngine;

public class SaveManager : SingletonBase<SaveManager>
{
    private const string _saveFileName = "PlayInfoData.json";
    private int _highScore;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        _highScore = -1;
    }
    
    private void Start()
    {
        LoadData();
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, _saveFileName);
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(_highScore);
        File.WriteAllText(GetSavePath(), json);
    }

    private void LoadData()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _highScore = JsonUtility.FromJson<int>(json);
        }
        else
        {
            InitializeDefaultData();
        }
    }

    private void InitializeDefaultData()
    {
        _highScore = 0;
        SaveData();
    }

    public void UpdatePlayInfo(int score)
    {
        if (score > _highScore)
        {
            _highScore = score;
        }
        SaveData();
    }

    public int GetPlayInfo()
    {
        return _highScore;
    }
}