using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : UIBase
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button retryButton;

    private void OnEnable()
    {
        SettingGameOverPopup();
        
        retryButton.onClick.AddListener((() =>
        {
            PlaySceneManager.Instance.RemoveAllActiveList();
            GameManager.Instance.GameStart();
        }));
    }
    
    private void SettingGameOverPopup()
    {
        scoreText.text = DataManager.Instance.RowCount.ToString();
        highScoreText.text = SaveManager.Instance.GetPlayInfo().ToString();
    }
}
