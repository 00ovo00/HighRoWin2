using System;
using TMPro;
using UnityEngine;

public class GameOverPopup : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText; // 현재 플레이한 점수
    [SerializeField] private TextMeshProUGUI highScoreText; // 최고점

    private void Awake()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();
        if (highScoreText == null)
            scoreText = GameObject.Find("HighScoreTxt").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SettingGameOverPopup();
    }
    
    private void SettingGameOverPopup()
    {
        scoreText.text = ScoreManager.Instance.RowCount.ToString();
        highScoreText.text = PlayDataManager.Instance.GetHighscore().ToString();
    }
}
