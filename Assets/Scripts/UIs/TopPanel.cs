using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    private const string LobbySceneName = "LobbyScene";
    
    [SerializeField] private TextMeshProUGUI curCoinText;
    [SerializeField] private TextMeshProUGUI curScoreText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button lobbyButton;

    private void OnEnable()
    {
        // connect the events to update the UI if there is a change in score or coin
        DataManager.Instance.OnCoinChanged -= UpdateCoinTxt;
        DataManager.Instance.OnCoinChanged += UpdateCoinTxt;
        DataManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        DataManager.Instance.OnScoreChanged += UpdateScoreTxt;
        
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        lobbyButton.onClick.AddListener(OnLobbyButtonClicked);
        
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

    // executed when click pause button
    private void OnPauseButtonClicked()
    {
        if (!GameManager.Instance.isPlaying) return;    // return if game is not playing
        
        Time.timeScale = 0; // time stop
        GameManager.Instance.isPlaying = false;
        
        UIManager.Instance.Show<PausePopup>();
        
        ToggleButtons(true);    // enable buttons on the top panal
    }

    // executed when click lobby button
    private void OnLobbyButtonClicked()
    {
        CharacterManager.Instance.ReSetCharacterObj();
        Time.timeScale = 1;
        SceneManager.LoadScene(LobbySceneName);
    }

    // toggle setting and lobby button
    public void ToggleButtons(bool isActive)
    {
        settingButton.gameObject.SetActive(isActive);
        lobbyButton.gameObject.SetActive(isActive);
    }
}
