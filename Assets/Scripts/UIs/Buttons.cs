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
        // 로비에서 플레이 버튼 누른 시점의 캐릭터 인덱스를 현재 인덱스로 갱신
        SaveManager.Instance.UpdateCurCharacterIdx(CharacterManager.Instance.curCharacterIdx);
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
