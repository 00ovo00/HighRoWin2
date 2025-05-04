using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : SingletonBase<GameManager>
{
    public Action OnGameOver;
    public bool IsPlaying { get; private set; }

    private void Start()
    {
        IsPlaying = true;
    }

    public void GameStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        IsPlaying = true;
    }
    
    public void GameOver()
    {
        IsPlaying = false;
        OnGameOver?.Invoke();
        SaveManager.Instance.UpdatePlayInfo(DataManager.Instance.RowCount);
        UIManager.Instance.Show<GameOverPopup>();
    }
}
