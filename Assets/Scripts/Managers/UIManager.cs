using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBase<UIManager>
{
    // set fixed resolution
    public float screenWidth = 720;
    public float screenHeight = 1280;

    [SerializeField] private List<UIBase> uiList = new List<UIBase>();  // list that manages popup UI

    // get an UI element from Resources folder and display on screen
    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();   // get the UI element name as its type name
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // get the UI element from Resources folder
        /* the class name and prefab name of the UI must be the same */
        if (go == null)
        {
            //Debug.Log($"UI Load Failed. {uiName} doesn't exist in Resources/UI/");
            return null;
        }
        var ui = Load<T>(go, uiName);
        uiList.Add(ui);
        return (T)ui;
    }

    // create the canvas and the UI element, and put UI on the canvas
    private T Load<T>(UIBase prefab, string uiName) where T : UIBase
    {
        GameObject newCanvasObject = new GameObject(uiName + "Canvas"); // create game object to make canvas

        var canvas = newCanvasObject.AddComponent<Canvas>();    // add canvas component
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;  // set canvas render mode

        // add canvas scaler component and set scale
        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);
        
        newCanvasObject.AddComponent<GraphicRaycaster>();   // add component for UI interact

        UIBase ui = Instantiate(prefab, newCanvasObject.transform); // instantiate UI as child of canvas
        ui.name = ui.name.Replace("(Clone)", "");   // delete (Clone) on its name
        ui.canvas = canvas; // put the UI on the canvas
        ui.canvas.sortingOrder = uiList.Count;  // put the latest UI on the top

        return (T)ui;
    }

    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();
        Hide(uiName);
    }

    public void Hide(string uiName)
    {
        UIBase go = uiList.Find(obj => obj.name == uiName); // find the UI name in the active UI list
        uiList.Remove(go);
        Destroy(go.canvas.gameObject);
    }
}