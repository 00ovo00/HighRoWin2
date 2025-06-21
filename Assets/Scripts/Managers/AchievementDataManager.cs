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
        Debug.Log(path);
        
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

    // 업적 초기화
    public void InitializeDefaultData()
    {
        _achievementData.achievements = new List<Achievement>();
        string newline = Environment.NewLine;
        
        // 최고점 달성 업적 (5개)
        _achievementData.achievements.Add(new Achievement("Spooky First Step", "100 Row! You’re just getting started!", AchievementType.HighScore, 100));
        _achievementData.achievements.Add(new Achievement("Ghostly Wanderer", "200 Row! Reached the haunted village gate.", AchievementType.HighScore, 200));
        _achievementData.achievements.Add(new Achievement("Night Crawler", "300 Row! Navigated the dark like a pro.", AchievementType.HighScore, 300));
        _achievementData.achievements.Add(new Achievement("Trick or Master", "500 Row! A true night hunter!", AchievementType.HighScore, 500));
        _achievementData.achievements.Add(new Achievement("Legend of Halloween", "1000 Row! A Halloween legend is born!", AchievementType.HighScore, 1000));
        
        // 누적 재화 달성 업적 (2개)
        _achievementData.achievements.Add(new Achievement("Candy Addict", "Collected 50,000 candies!" + newline + "Can’t live without sweets.", AchievementType.TotalCoin, 50000));
        _achievementData.achievements.Add(new Achievement("Candy Overlord", "Collected 100,000 candies!" + newline + "The ghosts bow to you!", AchievementType.TotalCoin, 100000));
        
        // 캐릭터 해금 업적 (3개)
        _achievementData.achievements.Add(new Achievement("First Summoning", "Summoned 2 ghost friends!", AchievementType.UnlockCharacters, 2));
        _achievementData.achievements.Add(new Achievement("Ghost Party", "Collected 4 ghosts!" + newline + "Time to throw a spooky party!", AchievementType.UnlockCharacters, 4));
        _achievementData.achievements.Add(new Achievement("Phantom Commander", "Unlocked 7 ghost friends!" + newline + "You're now their commander.", AchievementType.UnlockCharacters, 7));
        
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

    // 업적 해금했는지 확인
    private void CheckAchievementsByValue(AchievementType type, int currentValue)
    {
        bool hasNewAchievement = false; // 업적 해금 여부
        
        foreach (Achievement achievement in _achievementData.achievements)
        {
            if (achievement.Type == type && !achievement.IsCleared) // 업적 타입이 같고 해금되지 않았으면
            {
                if (currentValue >= achievement.RequiredValue)  // 현재 값이 업적 해금 요구값보다 크거나 같으면
                {   
                    achievement.IsCleared = true;   // 업적 해금한 상태로 변경

                    // 업적 달성 팝업 표시
                    SoundManager.Instance.PlayAchieveSFX();
                    AchieveAlertPopup popup = UIManager.Instance.Show<AchieveAlertPopup>();
                    if (popup != null)
                    {
                        popup.SetAchievementName(achievement.AchievementName);
                    }
                    
                    Debug.Log($"Achievement Unlocked! {achievement.AchievementName}: {achievement.AchievementDescription}");
                    hasNewAchievement = true;
                }
            }
        }
        // 새로 업적 해금했으면 저장
        if (hasNewAchievement)
        {
            SaveData();
        }
    }

    // 모든 업적 정보 반환
    public List<Achievement> GetAllAchievements()
    {
        return _achievementData.achievements;
    }
    
    // 업적 진행도 반환
    public float GetAchievementProgressByType(AchievementType type, int requiredvalue)
    {
        float progress = 0;
        switch (type)
        {
            case AchievementType.HighScore:
            {
                progress = PlayDataManager.Instance.GetHighscore() / (float)requiredvalue;
                if (progress >= 1) return 1;
                return progress;
            }
            case AchievementType.TotalCoin:
            {
                progress = PlayDataManager.Instance.GetTotalCoin() / (float)requiredvalue;
                if (progress >= 1) return 1;
                return progress;
            }
            case AchievementType.UnlockCharacters:
            {
                progress = PlayDataManager.Instance.GetUnlockedCharacterCount() / (float)requiredvalue;
                if (progress >= 1) return 1;
                return progress;
            }
        }
        return progress;
    }
}