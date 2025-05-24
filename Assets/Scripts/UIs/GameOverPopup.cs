using TMPro;
using UnityEngine;

public class GameOverPopup : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        SettingGameOverPopup();
    }
    
    private void SettingGameOverPopup()
    {
        scoreText.text = DataManager.Instance.RowCount.ToString();
        highScoreText.text = SaveManager.Instance.GetHighscore().ToString();
    }
}
