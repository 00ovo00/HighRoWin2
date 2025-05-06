using UnityEngine;
using UnityEngine.UI;

public class TitlePopup : UIBase
{
    [SerializeField] private Button startButton;
    
    private GameObject _topPanel;

    private void Start()
    {
        _topPanel = FindAnyObjectByType<TopPanel>().gameObject;
        
        startButton.onClick.AddListener(() =>
        {
            _topPanel.SetActive(true);
            UIManager.Instance.Hide<TitlePopup>();
        });
        
        _topPanel.SetActive(false);
    }
}
