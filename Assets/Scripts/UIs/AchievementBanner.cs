using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBanner : MonoBehaviour
{
    [SerializeField] private Image achievementImage;
    [SerializeField] private TextMeshProUGUI achievementNameText;
    [SerializeField] private TextMeshProUGUI achievementDiscriptionText;
    [SerializeField] private Slider sliderValue;

    public void SetAchievementBanner(bool isCleard, string achievementName, string achievementDiscription, float value)
    {
        if (!isCleard)
            achievementImage.color = Color.gray;
        achievementNameText.text = achievementName;
        achievementDiscriptionText.text = achievementDiscription;
        sliderValue.value = value;
    }
}
