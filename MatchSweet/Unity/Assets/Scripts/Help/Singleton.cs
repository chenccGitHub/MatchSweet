using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;

    private static object @lock = new object();

    public static T Instance
    {
        get
        {
            if ((Object)_instance != (Object)null)
            {
                return _instance;
            }

            lock (@lock)
            {
                _instance = Object.FindObjectOfType<T>();
                if ((Object)_instance != (Object)null)
                {
                    return _instance;
                }

                Debug.Log("Create " + typeof(T).Name + " Singleton");
                GameObject gameObject = new GameObject(typeof(T).Name);
                _instance = gameObject.AddComponent<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if ((Object)_instance == (Object)null)
        {
            _instance = (T)this;
        }
        else if (_instance != this)
        {
            Object.Destroy(this);
            return;
        }

        if (base.transform != Singleton<MonoSingletonRoot>.Instance.transform && base.transform.parent != Singleton<MonoSingletonRoot>.Instance.transform)
        {
            base.transform.SetParent(Singleton<MonoSingletonRoot>.Instance.transform);
        }

        if (base.transform.root == base.transform)
        {
            Object.DontDestroyOnLoad(base.gameObject);
        }
    }
}
public class MonoSingletonRoot : Singleton<MonoSingletonRoot>
{
}