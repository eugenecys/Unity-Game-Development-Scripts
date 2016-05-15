using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    // Looks for an instance of T in the Object space
                    // If found, that instance is used.
                    // Otherwise, it will create a new instance.
                    _instance = (T)FindObjectOfType(typeof(T));
                    
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                     
                }
                return _instance;
            }
        }
    }
}