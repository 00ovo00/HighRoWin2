using TMPro;
using UnityEngine;

public class PausePopup : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        SettingPausePopup();
    }
    
    private void SettingPausePopup()
    {
        scoreText.text = DataManager.Instance.RowCount.ToString();
        highScoreText.text = SaveManager.Instance.GetPlayInfo().ToString();
    }
}
