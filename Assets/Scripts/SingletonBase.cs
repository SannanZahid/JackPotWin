using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance {get; private set;}
    protected virtual void Awake()
    {
        Init();
    }

    internal void Init()
    {
        if(Instance is null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }
    }

    public virtual void OnDestroy()
    {
        Instance = null;
    }
}
