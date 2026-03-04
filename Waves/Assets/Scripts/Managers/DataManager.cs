using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    void Awake()
    {
        //criar isto estático
        instance = this;

        // meter para nao destruir entre cenas
        DontDestroyOnLoad(gameObject);
    }
}