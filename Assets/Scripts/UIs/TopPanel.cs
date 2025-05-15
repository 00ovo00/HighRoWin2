using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    private const string LobbySceneName = "LobbyScene";
    
    [SerializeField] private TextMeshProUGUI curScoreText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button lobbyButton;

    private void OnEnable()
    {
        DataManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        DataManager.Instance.OnScoreChanged += UpdateScoreTxt;
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        lobbyButton.onClick.AddListener(OnLobbyButtonClicked);
        
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

    private void OnLobbyButtonClicked()
    {
        CharacterManager.Instance.ReSetCharacterObj();
        Time.timeScale = 1;
        SceneManager.LoadScene(LobbySceneName);
    }

    public void ToggleButtons(bool isActive)
    {
        settingButton.gameObject.SetActive(isActive);
        lobbyButton.gameObject.SetActive(isActive);
    }
}
