using TMPro;
using UnityEngine;

public class GameOverPopup : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText; // 현재 플레이한 점수
    [SerializeField] private TextMeshProUGUI highScoreText; // 최고점

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
