using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    private const string PlaySceneName = "PlayScene";

    public bool isPlaying;  // check game is playing
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);        
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        isPlaying = true;
        UIManager.Instance.Show<TitlePopup>();
        // set the title popup to appear only at the beginning of the first play
    }

    public void GameStart()
    {
        SceneManager.LoadScene(PlaySceneName); 
        isPlaying = true;
        Time.timeScale = 1;
    }
    
    // called when player triggered with moving object
    public void GameOver()
    {
        isPlaying = false;
        // update score and coin, save game data
        SaveManager.Instance.UpdateGameData(DataManager.Instance.RowCount, DataManager.Instance.SweetCount);
        UIManager.Instance.Show<GameOverPopup>();
    }
}
