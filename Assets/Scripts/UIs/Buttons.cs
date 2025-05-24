using UnityEngine;

public class Buttons : MonoBehaviour
{
    // executed when click X button on the pop-up UI
    public void OnResumeBtnClicked()
    {
        Time.timeScale = 1;
        GameManager.Instance.isPlaying = true;
    }

    // executed when click play button on the lobby
    public void OnPlayBtnClicked()
    {
        // update the character index at the time of pressing to the current index
        SaveManager.Instance.UpdateCurCharacterIdx(CharacterManager.Instance.curCharacterIdx);
        GameManager.Instance.GameStart();
    }

    // executed when click retry button on the game over pop-up
    public void OnRetryBtnClicked()
    {
        CharacterManager.Instance.ReSetCharacterObj();
        PlaySceneManager.Instance.RemoveAllActiveList();
        GameManager.Instance.GameStart();
    }

    // executed when click exit button
    public void OnExitBtnClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        # else
            Application.Quit();
        #endif
    }

    // executed when click setting button
    public void OnSettingsBtnClicked()
    {
        UIManager.Instance.Show<SettingPopup>();
    }
}
