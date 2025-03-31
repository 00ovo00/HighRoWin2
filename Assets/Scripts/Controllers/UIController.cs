using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverBg;
    [SerializeField] private TextMeshProUGUI curScoreText;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private Button retryButton;
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver -= SettingGameOverPopup;
        GameManager.Instance.OnGameOver += SettingGameOverPopup;

        DataManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        DataManager.Instance.OnScoreChanged += UpdateScoreTxt;
        
        retryButton.onClick.AddListener((() =>
        {
            GameManager.Instance.GameStart();
        }));
    }

    private void Start()
    {
        gameOverBg.SetActive(false);
    }

    private void SettingGameOverPopup()
    {
        endScoreText.text = curScoreText.text.Substring(5);
        gameOverBg.SetActive(true);
    }
    
    private void UpdateScoreTxt()
    {
        curScoreText.text = $"Row: {DataManager.Instance.RowCount.ToString()}";
    }
}
