using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sweetTxt;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button achievementButton;
    
    [SerializeField] private CircularCameraController cameraController;

    private void OnEnable()
    {
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void Start()
    {
        sweetTxt.text = $"Sweet: {SaveManager.Instance.GetCurrentCoin().ToString()}";
    }

    private void OnPrevButtonClicked()
    {
        if (!cameraController.IsRotating)
            cameraController.RotateToPrev();
    }

    private void OnNextButtonClicked()
    {
        if (!cameraController.IsRotating)
            cameraController.RotateToNext();
    }

    private void OnDisable()
    {
        prevButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
    }
}