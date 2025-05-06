using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI curScoreText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button lobbyButton;

    private void OnEnable()
    {
        DataManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        DataManager.Instance.OnScoreChanged += UpdateScoreTxt;
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        
        settingButton.gameObject.SetActive(false);
        lobbyButton.gameObject.SetActive(false);
    }

    private void UpdateScoreTxt()
    {
        curScoreText.text = $"ROW: {DataManager.Instance.RowCount.ToString()}";
    }

    private void OnPauseButtonClicked()
    {
        if (!GameManager.Instance.isPlaying) return;
        
        Time.timeScale = 0;
        GameManager.Instance.isPlaying = false;
        
        UIManager.Instance.Show<PausePopup>();
        
        ToggleButtons(true);
    }

    public void ToggleButtons(bool isActive)
    {
        settingButton.gameObject.SetActive(isActive);
        lobbyButton.gameObject.SetActive(isActive);
    }
}
