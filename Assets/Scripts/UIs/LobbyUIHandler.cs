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
    [SerializeField] private TextMeshProUGUI buyText;   // 필요 재화 텍스트
    [Header("Bottom Panel")]
    [SerializeField] private Button achievementButton;
    
    [SerializeField] private CircularCameraController cameraController;

    private void OnEnable()
    {
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void Start()
    {
        sweetTxt.text = $"Sweet: {SaveManager.Instance.GetCurrentCoin().ToString()}";   // 현재 보유 재화 표시
        ToggleButtons();
    }

    // 이전 버튼 클릭 시 실행
    private void OnPrevButtonClicked()
    {
        if (!cameraController.IsRotating)   // 카메라 회전하고 있지 않으면
            cameraController.RotateToPrev();    // 이전 캐릭터 바라보도록 회전
        ToggleButtons();
    }

    // 다음 버튼 클릭 시 실행
    private void OnNextButtonClicked()
    {
        if (!cameraController.IsRotating)   // 카메라 회전하고 있지 않으면
            cameraController.RotateToNext();    // 다음 캐릭터 바라보도록 회전
        ToggleButtons();
    }

    // 구매 버튼 클릭 시 실행
    private void OnBuyButtonClicked()
    {
        CharacterManager.Instance.BuyCharacter();
    }

    // 현재 선택 중인 캐릭터 상태에 따라 버튼 토글
    private void ToggleButtons()
    {
        // 현재 선택한 캐릭터가 사용 가능한 상태면
        if (SaveManager.Instance.IsCharacterAvailable(CharacterManager.Instance.curCharacterIdx))
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
            buyText.text = CharacterManager.Instance.GetCharacterData(CharacterManager.Instance.curCharacterIdx).requiredSweet.ToString();
        }
    }

    private void OnDisable()
    {
        prevButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
    }
}