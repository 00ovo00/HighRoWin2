using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBanner : MonoBehaviour
{
    [SerializeField] private Image achievementImage;
    [SerializeField] private TextMeshProUGUI achievementNameText;
    [SerializeField] private TextMeshProUGUI achievementDiscriptionText;
    [SerializeField] private Slider sliderValue;

    private void Awake()
    {
        if (achievementImage == null)
            achievementImage = GetComponent<Image>();
        if (achievementNameText == null)
            achievementNameText = GameObject.Find("AchievementNameTxt").GetComponent<TextMeshProUGUI>();
        if (achievementDiscriptionText == null)
            achievementDiscriptionText = GameObject.Find("AchievementDiscriptionTxt").GetComponent<TextMeshProUGUI>();
        if (sliderValue == null)
            sliderValue = GetComponentInChildren<Slider>();
    }

    public void SetAchievementBanner(bool isCleard, string achievementName, string achievementDiscription, float value)
    {
        if (!isCleard)
            achievementImage.color = Color.gray;
        achievementNameText.text = achievementName;
        achievementDiscriptionText.text = achievementDiscription;
        sliderValue.value = value;
    }
}
