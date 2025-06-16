using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBanner : MonoBehaviour
{
    [SerializeField] private bool isEnabled;
    [SerializeField] private TextMeshProUGUI achievementNameText;
    [SerializeField] private TextMeshProUGUI achievementDiscriptionText;
    [SerializeField] private Slider sliderValue;

    public void SetAchievementBanner(bool isCleard, string achievementName, string achievementDiscription, float value)
    {
        isEnabled = isCleard;
        achievementNameText.text = achievementName;
        achievementDiscriptionText.text = achievementDiscription;
        sliderValue.value = value;
    }
}
