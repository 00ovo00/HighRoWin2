using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    public Action OnGameOver;
    private bool _isPlaying = false;

    public void GameStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _isPlaying = true;
        Time.timeScale = 1.0f;
    }
    
    public void GameOver()
    {
        _isPlaying = false;
        OnGameOver?.Invoke();
    }
}
