using UnityEngine;
using UnityEngine.UI;

public class TitlePopup : UIBase
{
    [SerializeField] private Button startButton;    // set the start button to full screen size
    
    private GameObject _topPanel;

    private void Start()
    {
        _topPanel = FindAnyObjectByType<TopPanel>().gameObject;
        
        // touch the screen to activate the top panel and disable the title pop-up
        startButton.onClick.AddListener(() =>
        {
            _topPanel.SetActive(true);
            UIManager.Instance.Hide<TitlePopup>();
        });
        
        _topPanel.SetActive(false);
    }
}
