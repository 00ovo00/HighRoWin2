using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sweetTxt;
    [SerializeField] private GameObject playButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button achievementButton;
    
    [SerializeField] private TextMeshProUGUI buyText;
    
    [SerializeField] private CircularCameraController cameraController;

    private void OnEnable()
    {
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void Start()
    {
        sweetTxt.text = $"Sweet: {SaveManager.Instance.GetCurrentCoin().ToString()}";
        ToggleButtons();
    }

    private void OnPrevButtonClicked()
    {
        if (!cameraController.IsRotating)
            cameraController.RotateToPrev();
        ToggleButtons();
    }

    private void OnNextButtonClicked()
    {
        if (!cameraController.IsRotating)
            cameraController.RotateToNext();
        ToggleButtons();
    }

    private void ToggleButtons()
    {
        if (SaveManager.Instance.IsCharacterAvailable(CharacterManager.Instance.curCharacterIdx))
        {
            playButton.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }
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
    }
}