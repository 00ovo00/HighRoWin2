using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : UIBase
{
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    
    private TopPanel _topPanel; // 상단 패널 버튼 토글을 위해 참조 연결

    private void Awake()
    {
        _topPanel = FindAnyObjectByType<TopPanel>();
    }

    private void OnEnable()
    {
        SettingPausePopup();
        // 플레이 버튼 클릭 시 상단 패널 버튼 비활성화하고 일시정지 팝업 없애기
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
