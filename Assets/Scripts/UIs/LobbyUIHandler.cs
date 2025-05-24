using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour
{
    [Header("Top Panel")]
    [SerializeField] private TextMeshProUGUI sweetTxt;  // display current coin amount
    [Header("Middle Panel")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI buyText;   // display needed coin to buy
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
        sweetTxt.text = $"Sweet: {SaveManager.Instance.GetCurrentCoin().ToString()}";
        ToggleButtons();
    }

    // executed when click previous button
    private void OnPrevButtonClicked()
    {
        if (!cameraController.IsRotating)   // if camera is not rotating
            cameraController.RotateToPrev();    // rotate towards previous character
        ToggleButtons();
    }

    // executed when click next button
    private void OnNextButtonClicked()
    {
        if (!cameraController.IsRotating)   // if camera is not rotating
            cameraController.RotateToNext();    // rotate towards next character
        ToggleButtons();
    }

    // executed when click buy button
    private void OnBuyButtonClicked()
    {
        CharacterManager.Instance.BuyCharacter();
    }

    // toggle button depending on the status of the currently selected character
    private void ToggleButtons()
    {
        // if the currently selected character is available
        if (SaveManager.Instance.IsCharacterAvailable(CharacterManager.Instance.curCharacterIdx))
        {
            playButton.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
        // if the currently selected character is unavailable
        else
        {
            playButton.SetActive(false);
            buyButton.gameObject.SetActive(true);
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