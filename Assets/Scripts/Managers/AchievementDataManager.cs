using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

// 업적 데이터 클래스
[System.Serializable]
public class Achievement
{
    public bool IsCleared;  // 업적 해금 체크
    public string AchievementName;  // 업적 이름
    public string AchievementDescription;   // 업적 설명
    public AchievementType Type;    // 업적 타입
    public int RequiredValue;   // 달성 조건 값
    
    public Achievement(string name, string description, AchievementType type, int requiredValue)
    {
        IsCleared = false;
        AchievementName = name;
        AchievementDescription = description;
        Type = type;
        RequiredValue = requiredValue;
    }
}

// 업적 타입 열거형
public enum AchievementType
{
    HighScore,      // 최고점 달성
    TotalCoin,      // 누적 재화 달성
    UnlockCharacters    // 캐릭터 해금
}

// 업적 저장 데이터
[System.Serializable]
public class AchievementSaveData
{
    public List<Achievement> achievements;
    
    public AchievementSaveData()
    {
        achievements = new List<Achievement>();
    }
}

public class AchievementDataManager : SingletonBase<AchievementDataManager>, ISaveSystem
{
    private const string SaveFileName = "AchievementData.json";
    private AchievementSaveData _achievementData = new AchievementSaveData();
    
    // 업적 해금 이벤트
    public static event Action<Achievement> OnAchievementUnlocked;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        LoadData();
        UnsubscribeFromEvents();
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    // 이벤트 구독
    private void SubscribeToEvents()
    {
        PlayDataManager.OnHighScoreUpdated += CheckHighScoreAchievements;
        PlayDataManager.OnTotalCoinUpdated += CheckTotalCoinAchievements;
        PlayDataManager.OnCharacterUnlocked += CheckCharacterAchievements;
    }

    private void UnsubscribeFromEvents()
    {
        PlayDataManager.OnHighScoreUpdated -= CheckHighScoreAchievements;
        PlayDataManager.OnTotalCoinUpdated -= CheckTotalCoinAchievements;
        PlayDataManager.OnCharacterUnlocked -= CheckCharacterAchievements;
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(_achievementData);
        File.WriteAllText(GetSavePath(), json);
    }

    public void LoadData()
    {
        string path = GetSavePath();
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _achievementData = JsonUtility.FromJson<AchievementSaveData>(json);
            
            if (_achievementData.achievements == null || _achievementData.achievements.Count == 0)
            {
                InitializeDefaultData();
            }
        }
        else
        {
            InitializeDefaultData();
        }
    }

    public void InitializeDefaultData()
    {
        _achievementData.achievements = new List<Achievement>();
        
        // 최고점 달성 업적 (5개)
        _achievementData.achievements.Add(new Achievement("Spooky First Step", "100 Row! You’re just getting started!", AchievementType.HighScore, 100));
        _achievementData.achievements.Add(new Achievement("Ghostly Wanderer", "500 Row! Reached the haunted village gate.", AchievementType.HighScore, 500));
        _achievementData.achievements.Add(new Achievement("Night Crawler", "1000 Row! Navigated the dark like a pro.", AchievementType.HighScore, 1000));
        _achievementData.achievements.Add(new Achievement("Trick or Master", "5000 Row! A true night hunter!", AchievementType.HighScore, 5000));
        _achievementData.achievements.Add(new Achievement("Legend of Halloween", "10000 Row! A Halloween legend is born!", AchievementType.HighScore, 10000));
        
        // 누적 재화 달성 업적 (2개)
        _achievementData.achievements.Add(new Achievement("Candy Addict", "Collected 500,000 candies! Can’t live without sweets.", AchievementType.TotalCoin, 500000));
        _achievementData.achievements.Add(new Achievement("Candy Overlord", "Collected 1,000,000 candies! The ghosts bow to you!", AchievementType.TotalCoin, 1000000));
        
        // 캐릭터 해금 업적 (3개)
        _achievementData.achievements.Add(new Achievement("First Summoning", "Summoned 2 ghost friends!", AchievementType.UnlockCharacters, 2));
        _achievementData.achievements.Add(new Achievement("Ghost Party", "Collected 4 ghosts! Time to throw a spooky party!", AchievementType.UnlockCharacters, 4));
        _achievementData.achievements.Add(new Achievement("Phantom Commander", "Unlocked 7 ghost friends! You're now their commander.", AchievementType.UnlockCharacters, 7));
        
        SaveData();
    }

    // 이벤트 핸들러
    private void CheckHighScoreAchievements(int highScore)
    {
        CheckAchievementsByValue(AchievementType.HighScore, highScore);
    }

    private void CheckTotalCoinAchievements(int totalCoin)
    {
        CheckAchievementsByValue(AchievementType.TotalCoin, totalCoin);
    }

    private void CheckCharacterAchievements(int unlockedCount)
    {
        CheckAchievementsByValue(AchievementType.UnlockCharacters, unlockedCount);
    }

    private void CheckAchievementsByValue(AchievementType type, int currentValue)
    {
        bool hasNewAchievement = false;
        
        foreach (Achievement achievement in _achievementData.achievements)
        {
            if (achievement.Type == type && !achievement.IsCleared)
            {
                if (currentValue >= achievement.RequiredValue)
                {
                    achievement.IsCleared = true;
                    OnAchievementUnlocked?.Invoke(achievement);
                    Debug.Log($"Achievement Unlocked! {achievement.AchievementName}: {achievement.AchievementDescription}");
                    hasNewAchievement = true;
                }
            }
        }
        
        if (hasNewAchievement)
        {
            SaveData();
        }
    }

    public List<Achievement> GetAllAchievements()
    {
        return _achievementData.achievements;
    }
    
    public List<Achievement> GetAchievementsByType(AchievementType type)
    {
        List<Achievement> result = new List<Achievement>();
        foreach (Achievement achievement in _achievementData.achievements)
        {
            if (achievement.Type == type)
                result.Add(achievement);
        }
        return result;
    }
    
    public int GetClearedAchievementCount()
    {
        int count = 0;
        foreach (Achievement achievement in _achievementData.achievements)
        {
            if (achievement.IsCleared)
                count++;
        }
        return count;
    }
    
    public int GetClearedAchievementCountByType(AchievementType type)
    {
        int count = 0;
        foreach (Achievement achievement in _achievementData.achievements)
        {
            if (achievement.Type == type && achievement.IsCleared)
                count++;
        }
        return count;
    }
    
    public int GetTotalAchievementCount()
    {
        return _achievementData.achievements.Count;
    }
    
    public int GetTotalAchievementCountByType(AchievementType type)
    {
        int count = 0;
        foreach (Achievement achievement in _achievementData.achievements)
        {
            if (achievement.Type == type)
                count++;
        }
        return count;
    }
    
    public float GetAchievementProgressByType(AchievementType type, int requiredvalue)
    {
        float progress = 0;
        switch (type)
        {
            case AchievementType.HighScore:
            {
                progress = PlayDataManager.Instance.GetHighscore() / requiredvalue;
                if (progress >= 1) return 1;
                else return progress;
            }
            case AchievementType.TotalCoin:
            {
                progress = PlayDataManager.Instance.GetTotalCoin() / requiredvalue;
                if (progress >= 1) return 1;
                else return progress;
            }
            case AchievementType.UnlockCharacters:
            {
                progress = PlayDataManager.Instance.GetUnlockedCharacterCount() / requiredvalue;
                if (progress >= 1) return 1;
                else return progress;
            }
        }
        return progress;
    }
}