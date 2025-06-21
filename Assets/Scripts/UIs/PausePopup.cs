using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : UIBase
{
    [SerializeField] private Button playButton; // X 버튼(멈춘 상태에서 다시 재개)
    [SerializeField] private TextMeshProUGUI scoreText; // 현재 점수
    [SerializeField] private TextMeshProUGUI highScoreText; // 최고점
    
    private TopPanel _topPanel; // 상단 패널 버튼 토글을 위해 참조 연결

    private void Awake()
    {
        if (playButton == null)
            playButton = GameObject.Find("PlayBtn").GetComponent<Button>();
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();
        if (highScoreText == null)
            highScoreText = GameObject.Find("HighScoreTxt").GetComponent<TextMeshProUGUI>();
        
        _topPanel = FindAnyObjectByType<TopPanel>();
    }

    private void OnEnable()
    {
        SettingPausePopup();
        // 플레이 버튼 클릭 시 상단 패널 버튼 비활성화하고 일시정지 팝업 없애기
        playButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClickSFX();
            _topPanel.ToggleButtons(false);
            UIManager.Instance.Hide<PausePopup>();
        });
    }
    
    private void SettingPausePopup()
    {
        scoreText.text = ScoreManager.Instance.RowCount.ToString();
        highScoreText.text = PlayDataManager.Instance.GetHighscore().ToString();
    }
}
