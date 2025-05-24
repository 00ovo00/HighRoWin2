using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : UIBase
{
    [SerializeField] private Button playButton; // X button(resume)
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    
    private TopPanel _topPanel; // reference for top panel button toggle

    private void Awake()
    {
        _topPanel = FindAnyObjectByType<TopPanel>();
    }

    private void OnEnable()
    {
        SettingPausePopup();
        // when play button clicked, disable buttons on the top panel and hide pause pop-up
        playButton.onClick.AddListener(() =>
        {
            _topPanel.ToggleButtons(false);
            UIManager.Instance.Hide<PausePopup>();
        });
    }
    
    private void SettingPausePopup()
    {
        scoreText.text = DataManager.Instance.RowCount.ToString();
        highScoreText.text = SaveManager.Instance.GetHighscore().ToString();
    }
}
