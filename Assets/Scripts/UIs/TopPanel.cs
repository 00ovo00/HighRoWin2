using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TopPanel : MonoBehaviour
{
    private const string LobbySceneName = "LobbyScene";
    
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI curCoinText;   // 현재 획득 재화
    [SerializeField] private TextMeshProUGUI curScoreText;  // 현재 점수
    [SerializeField] private Button pauseButton;    // 일시정지 버튼
    [SerializeField] private Button settingButton;  // 설정 버튼
    [SerializeField] private Button lobbyButton;    // 로비로 이동하는 버튼

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f; // 애니메이션 지속 시간
    [SerializeField] private float buttonOffsetY = 150f;     // 버튼이 내려올 Y축 거리
    
    // 버튼 원위치
    private Vector2 settingButtonOriginPos;
    private Vector2 lobbyButtonOriginPos;
    
    private void Awake()
    {
        // 버튼 원위치 설정
        var settingButtonRect = settingButton.GetComponent<RectTransform>();
        settingButtonOriginPos = settingButtonRect.anchoredPosition;
        var lobbyButtonRect = lobbyButton.GetComponent<RectTransform>();
        lobbyButtonOriginPos = lobbyButtonRect.anchoredPosition;

        // 버튼 시작 위치로 이동
        settingButtonRect.anchoredPosition = new Vector2(settingButtonOriginPos.x, settingButtonOriginPos.y + buttonOffsetY);
        lobbyButtonRect.anchoredPosition = new Vector2(lobbyButtonOriginPos.x, lobbyButtonOriginPos.y + buttonOffsetY);
    }
    
    private void OnEnable()
    {
        // 점수나 재화 변동 있으면 UI 갱신하도록 이벤트 연결
        ScoreManager.Instance.OnCoinChanged -= UpdateCoinTxt;
        ScoreManager.Instance.OnCoinChanged += UpdateCoinTxt;
        ScoreManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        ScoreManager.Instance.OnScoreChanged += UpdateScoreTxt;
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        lobbyButton.onClick.AddListener(OnLobbyButtonClicked);
        
        // 시작 시 설정 버튼과 로비 버튼 투명하게 만들어 보이지 않게 처리
        settingButton.image.color = new Color(1, 1, 1, 0);
        lobbyButton.image.color = new Color(1, 1, 1, 0);
        
        // 상호작용 비활성화
        settingButton.interactable = false;
        lobbyButton.interactable = false;
    }

    private void UpdateCoinTxt()
    {
        curCoinText.text = ScoreManager.Instance.SweetCount.ToString();
    }
    
    private void UpdateScoreTxt()
    {
        curScoreText.text = $"ROW: {ScoreManager.Instance.RowCount.ToString()}";
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
        var settingButtonRect = settingButton.GetComponent<RectTransform>();
        var lobbyButtonRect = lobbyButton.GetComponent<RectTransform>();

        if (isActive)
        {
            // 버튼을 활성화하고 원래 위치로 이동시키는 애니메이션
            settingButton.interactable = true;
            lobbyButton.interactable = true;
            
            settingButton.image.DOFade(1f, animationDuration).SetUpdate(true);
            settingButtonRect.DOAnchorPos(settingButtonOriginPos, animationDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);

            lobbyButton.image.DOFade(1f, animationDuration).SetUpdate(true);
            lobbyButtonRect.DOAnchorPos(lobbyButtonOriginPos, animationDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }
        else
        {
            // 버튼을 비활성화하고 원래 위치(화면 위)로 되돌리는 애니메이션
            settingButton.interactable = false;
            lobbyButton.interactable = false;

            settingButton.image.DOFade(0f, animationDuration).SetUpdate(true);
            settingButtonRect.DOAnchorPos(new Vector2(settingButtonOriginPos.x, settingButtonOriginPos.y + buttonOffsetY), animationDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true);

            lobbyButton.image.DOFade(0f, animationDuration).SetUpdate(true);
            lobbyButtonRect.DOAnchorPos(new Vector2(lobbyButtonOriginPos.x, lobbyButtonOriginPos.y + buttonOffsetY), animationDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
    }
}
