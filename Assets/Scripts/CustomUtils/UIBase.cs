using UnityEngine;

public class UIBase : MonoBehaviour
{
    public Canvas canvas;   // make the classes inherit from UIBase have default canvas

    public void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }
}