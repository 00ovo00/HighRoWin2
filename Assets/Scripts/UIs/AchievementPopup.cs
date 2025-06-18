using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : UIBase
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private VerticalLayoutGroup content;

    private List<Achievement> _achievementList = new List<Achievement>();
    private void Awake()
    {
        SetAchievementList();
    }
    
    private void OnEnable()
    {
        closeBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClickSFX();
            UIManager.Instance.Hide<AchievementPopup>();
        });
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveAllListeners();
    }

    private void SetAchievementList()
    {
        _achievementList = AchievementDataManager.Instance.GetAllAchievements();

        for (int i = 0; i < _achievementList.Count; i++)
        {
            Achievement achievement = _achievementList[i];
            GameObject achievementObj = Instantiate(Resources.Load<GameObject>("UI/AchievementBanner"), content.transform, true);
            AchievementBanner banner = achievementObj.GetComponent<AchievementBanner>();

            float achievementProgress = AchievementDataManager.Instance.GetAchievementProgressByType(achievement.Type, achievement.RequiredValue);
            banner.SetAchievementBanner(achievement.IsCleared, achievement.AchievementName, achievement.AchievementDescription, achievementProgress);
        }
    }
}
