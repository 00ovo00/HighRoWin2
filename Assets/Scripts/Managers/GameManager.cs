using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    private const string PlaySceneName = "PlayScene";

    public bool isPlaying;  // 게임 실행 중인지 확인하는 플래그
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);        
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        isPlaying = true;   // 게임 실행 상태로 전환
        UIManager.Instance.Show<TitlePopup>();  // 타이틀 팝업 띄우기
        // 타이틀 팝업은 최초 플레이 시작에만 뜨도록 설정
    }

    // PlayBtn, RetryBtn 클릭 시 실행
    public void GameStart()
    {
        SceneManager.LoadScene(PlaySceneName);  // 플레이씬 로드
        isPlaying = true;   // 게임 실행 상태로 전환
        Time.timeScale = 1; // 시간 정속으로 흐르게 설정
    }
    
    // 플레이어가 움직이는 오브젝트와 트리거되면 호출
    public void GameOver()
    {
        isPlaying = false;  // 게임 미진행 상태로 전환
        // 점수와 코인 개수 갱신하여 저장
        SaveManager.Instance.UpdateGameData(DataManager.Instance.RowCount, DataManager.Instance.SweetCount);
        UIManager.Instance.Show<GameOverPopup>();   // 게임 종료창 띄우기
    }
}
