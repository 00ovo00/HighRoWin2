using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance    // allow singleton instance to be accessed directly from others
    {
        get
        {
            if (_instance != null)  // if instance exists already
                return _instance;   // return existing instance

            // if there is a T type object in scene, set it as an instance
            _instance = FindFirstObjectByType<T>();

            if (_instance == null)  // if there isn't a T type object in scene
            {
                // create new T type gameobject and add component
                GameObject go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)  // if there is no instance
        {
            _instance = this as T;  // set the currently created instance to a singleton instance
        }
        else if (_instance != this) // if existing instance and currently created instance are different
        {
            Destroy(gameObject);    // destroy currently created instance(keep existing instance)
        }
    }
}