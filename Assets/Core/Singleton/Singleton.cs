
using UnityEngine;

public class Singleton<T> : ComponentBehavior where T : ComponentBehavior
{
    private static T instance;
  
    public static T Instance => instance;

    protected override void Awake()
    {
        if (instance == null) instance = (T)(MonoBehaviour)this;
        else Destroy(gameObject);

        base.Awake();
        
    }
}
