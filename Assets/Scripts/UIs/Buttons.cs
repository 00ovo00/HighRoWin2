using System;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void OnResumeBtnClicked()
    {
        Time.timeScale = 1;
        GameManager.Instance.isPlaying = true;
    }

    public void OnPlayBtnClicked()
    {
        GameManager.Instance.GameStart();
    }

    public void OnRetryBtnClicked()
    {
        CharacterManager.Instance.ReSetCharacterObj();
        PlaySceneManager.Instance.RemoveAllActiveList();
        GameManager.Instance.GameStart();
    }

    public void OnExitBtnClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        # else
            Application.Quit();
        #endif
    }

    public void OnSettingsBtnClicked()
    {
        UIManager.Instance.Show<SettingPopup>();
    }
}
