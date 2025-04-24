using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverPopup : UIBase
{
    [SerializeField] private TextMeshProUGUI endScoreText;
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
        endScoreText.text = DataManager.Instance.RowCount.ToString();
    }
}
