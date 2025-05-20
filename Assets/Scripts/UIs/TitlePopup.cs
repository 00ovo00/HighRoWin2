using UnityEngine;
using UnityEngine.UI;

public class TitlePopup : UIBase
{
    [SerializeField] private Button startButton;    // 화면 전체 크기로 시작 버튼 설정
    
    private GameObject _topPanel;

    private void Start()
    {
        _topPanel = FindAnyObjectByType<TopPanel>().gameObject;
        
        // 화면을 터치하면 상단 패널 활성화하고 타이틀 팝업 비활성화
        startButton.onClick.AddListener(() =>
        {
            _topPanel.SetActive(true);
            UIManager.Instance.Hide<TitlePopup>();
        });
        
        _topPanel.SetActive(false); // 상단 패널 시작 시 비활성화
    }
}
