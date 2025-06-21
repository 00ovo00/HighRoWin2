using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour
{
    [Header("Top Panel")]
    [SerializeField] private TextMeshProUGUI sweetTxt;  // 보유하고 있는 재화 표시
    [Header("Middle Panel")]
    [SerializeField] private GameObject playButton; // 게임 시작 버튼
    [SerializeField] private Button buyButton;  // 구매 버튼
    [SerializeField] private Button prevButton; // 이전 캐릭터 선택
    [SerializeField] private Button nextButton; // 다음 캐릭터 선택
    [SerializeField] private TextMeshProUGUI buyTxt;   // 필요 재화 텍스트
    [Header("Bottom Panel")]
    [SerializeField] private Button achievementButton;
    
    [Header("Camera")]
    [SerializeField] private CircularCameraController cameraController;

    private void Awake()
    {
        if (sweetTxt == null)
            sweetTxt = GameObject.Find("SweetTxt").GetComponent<TextMeshProUGUI>();
        if (playButton == null)
            playButton = GameObject.Find("PlayBtn");
        if (buyButton == null)
            buyButton = GameObject.Find("BuyBtn").GetComponent<Button>();
        if (prevButton == null)
            prevButton = GameObject.Find("PrevBtn").GetComponent<Button>();
        if (nextButton == null)
            nextButton = GameObject.Find("NextBtn").GetComponent<Button>();
        if (buyTxt == null)
            buyTxt = GameObject.Find("BuyTxt").GetComponent<TextMeshProUGUI>();
        if (achievementButton == null)
            achievementButton = GameObject.Find("AchievementBtn").GetComponent<Button>();
        if (cameraController == null && Camera.main != null)
            cameraController = Camera.main.GetComponent<CircularCameraController>();
    }

    private void OnEnable()
    {
        UnsubscribeEvent();
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        achievementButton.onClick.AddListener(OnAchievementButtonClicked);
    }
    
    private void OnDisable()
    {
        UnsubscribeEvent();
    }

    private void Start()
    {
        sweetTxt.text = $"Sweet: {PlayDataManager.Instance.GetCurrentCoin().ToString()}";   // 현재 보유 재화 표시
        ToggleButtons();
    }

    // 이전 버튼 클릭 시 실행
    private void OnPrevButtonClicked()
    {
        if (!cameraController.IsRotating) // 카메라 회전하고 있지 않으면
        {
            SoundManager.Instance.PlayClickSFX();
            cameraController.RotateToPrev();    // 이전 캐릭터 바라보도록 회전
        }
        ToggleButtons();
    }

    // 다음 버튼 클릭 시 실행
    private void OnNextButtonClicked()
    {
        if (!cameraController.IsRotating) // 카메라 회전하고 있지 않으면
        {
            SoundManager.Instance.PlayClickSFX();
            cameraController.RotateToNext();    // 다음 캐릭터 바라보도록 회전
        }
        ToggleButtons();
    }

    // 구매 버튼 클릭 시 실행
    private void OnBuyButtonClicked()
    {
        SoundManager.Instance.PlayClickSFX();
        CharacterManager.Instance.BuyCharacter();
    }
    
    // 업적 버튼 클릭 시 실행
    private void OnAchievementButtonClicked()
    {
        SoundManager.Instance.PlayClickSFX();
        UIManager.Instance.Show<AchievementPopup>();
    }

    // 현재 선택 중인 캐릭터 상태에 따라 버튼 토글
    private void ToggleButtons()
    {
        // 현재 선택한 캐릭터가 사용 가능한 상태면
        if (PlayDataManager.Instance.IsCharacterAvailable(CharacterManager.Instance.curCharacterIdx))
        {
            playButton.SetActive(true); // 플레이 버튼 활성화
            buyButton.gameObject.SetActive(false);  // 구매 버튼 비활성화
        }
        // 현재 선택한 캐릭터가 사용 불가한 상태면
        else
        {
            playButton.SetActive(false); // 플레이 버튼 비활성화
            buyButton.gameObject.SetActive(true);  // 구매 버튼 활성화
            // 캐릭터를 보유하기 위해 필요한 재화량 표시
            buyTxt.text = CharacterManager.Instance.GetCharacterData(CharacterManager.Instance.curCharacterIdx).requiredSweet.ToString();
        }
    }

    private void UnsubscribeEvent()
    {
        prevButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
        achievementButton.onClick.RemoveAllListeners();
    }
}