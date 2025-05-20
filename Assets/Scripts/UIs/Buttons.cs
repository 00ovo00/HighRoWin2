using UnityEngine;

public class Buttons : MonoBehaviour
{
    // 팝업창에서 X 버튼 누르면 실행
    public void OnResumeBtnClicked()
    {
        Time.timeScale = 1;
        GameManager.Instance.isPlaying = true;
    }

    // 로비에서 플레이 버튼 누르면 실행
    public void OnPlayBtnClicked()
    {
        // 누른 시점의 캐릭터 인덱스를 현재 인덱스로 갱신
        SaveManager.Instance.UpdateCurCharacterIdx(CharacterManager.Instance.curCharacterIdx);
        GameManager.Instance.GameStart();
    }

    // 게임 종료 팝업창에서 재시작 버튼 누르면 실행
    public void OnRetryBtnClicked()
    {
        CharacterManager.Instance.ReSetCharacterObj();
        PlaySceneManager.Instance.RemoveAllActiveList();
        GameManager.Instance.GameStart();
    }

    // 종료 버튼 누르면 실행
    public void OnExitBtnClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        # else
            Application.Quit();
        #endif
    }

    // 설정 버튼 누르면 실행
    public void OnSettingsBtnClicked()
    {
        UIManager.Instance.Show<SettingPopup>();    // 설정창 팝업
    }
}
