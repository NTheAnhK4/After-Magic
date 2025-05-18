
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
  
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance == null) instance = (T)(MonoBehaviour)this;
        else Destroy(gameObject);

        
        
    }
}
