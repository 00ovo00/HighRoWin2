using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    private const string LobbySceneName = "LobbyScene";
    
    [SerializeField] private TextMeshProUGUI curCoinText;   // 현재 획득 재화
    [SerializeField] private TextMeshProUGUI curScoreText;  // 현재 점수
    [SerializeField] private Button pauseButton;    // 일시정지 버튼
    [SerializeField] private Button settingButton;  // 설정 버튼
    [SerializeField] private Button lobbyButton;    // 로비로 이동하는 버튼

    private void OnEnable()
    {
        // 점수나 재화 변동 있으면 UI 갱신하도록 이벤트 연결
        DataManager.Instance.OnCoinChanged -= UpdateCoinTxt;
        DataManager.Instance.OnCoinChanged += UpdateCoinTxt;
        DataManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        DataManager.Instance.OnScoreChanged += UpdateScoreTxt;
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        lobbyButton.onClick.AddListener(OnLobbyButtonClicked);
        
        // 시작 시 설정 버튼과 로비 버튼 비활성화
        settingButton.gameObject.SetActive(false);
        lobbyButton.gameObject.SetActive(false);
    }

    private void UpdateCoinTxt()
    {
        curCoinText.text = DataManager.Instance.SweetCount.ToString();
    }
    
    private void UpdateScoreTxt()
    {
        curScoreText.text = $"ROW: {DataManager.Instance.RowCount.ToString()}";
    }

    // 일시정지 버튼 클릭 시 실행
    private void OnPauseButtonClicked()
    {
        if (!GameManager.Instance.isPlaying) return;    // 게임 실행 중이 아니면 바로 리턴
        
        Time.timeScale = 0; // 게임 시간 멈춤
        GameManager.Instance.isPlaying = false; // 게임 실행 중이 아닌 상태로 전환
        
        UIManager.Instance.Show<PausePopup>();  // 일시정지창 팝업
        
        ToggleButtons(true);    // 상단 패널 버튼 활성화
    }

    // 로비 버튼 클릭 시 실행
    private void OnLobbyButtonClicked()
    {
        CharacterManager.Instance.ReSetCharacterObj();  // 캐릭터 초기화
        Time.timeScale = 1; // 게임 시간 정속으로 흐르게 설정
        SceneManager.LoadScene(LobbySceneName); // 로비로 이동
    }

    // 상단 패널 버튼(설정, 로비 버튼) 토글
    public void ToggleButtons(bool isActive)
    {
        settingButton.gameObject.SetActive(isActive);
        lobbyButton.gameObject.SetActive(isActive);
    }
}
