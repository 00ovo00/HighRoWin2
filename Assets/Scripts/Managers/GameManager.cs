using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : SingletonBase<GameManager>
{
    private const string PlaySceneName = "PlayScene";

    public bool isPlaying;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);        
    }

    private void Start()
    {
        isPlaying = true;
        UIManager.Instance.Show<TitlePopup>();
    }

    public void GameStart()
    {
        SceneManager.LoadScene(PlaySceneName);
        isPlaying = true;
        Time.timeScale = 1;
    }
    
    public void GameOver()
    {
        isPlaying = false;
        SaveManager.Instance.UpdateHighScore(DataManager.Instance.RowCount);
        UIManager.Instance.Show<GameOverPopup>();
    }
}
