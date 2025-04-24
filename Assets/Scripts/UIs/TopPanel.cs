using TMPro;
using UnityEngine;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI curScoreText;

    private void OnEnable()
    {
        DataManager.Instance.OnScoreChanged -= UpdateScoreTxt;
        DataManager.Instance.OnScoreChanged += UpdateScoreTxt;
    }
    
    private void UpdateScoreTxt()
    {
        curScoreText.text = $"ROW: {DataManager.Instance.RowCount.ToString()}";
    }
}
