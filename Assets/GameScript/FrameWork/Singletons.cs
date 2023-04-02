using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISingleton
{
    void OnRelease();
}

public sealed class SingletonMgr
{
    public static List<ISingleton> singletons = new List<ISingleton>();

    public static void AddSingleton(ISingleton singleton)
    {
        if (singletons.IndexOf(singleton) == -1)
        {
            singletons.Add(singleton);
        }
    }

    public static void Destroy()
    {
        int count = singletons.Count;
        for (int index = count - 1; index >= 0; index--)
        {
            singletons[index].OnRelease();
            singletons.RemoveAt(index);
        }
    }
}



public abstract class LogicSingleton<T> : ISingleton where T : ISingleton, new()
{
    public bool IsPersist { get; set; }

    private static T instance;
    public static T Instance
    {
        get
        {
            if (Equals(instance, default(T)))
            {
                instance = new T();
                SingletonMgr.AddSingleton(instance);
            }
            return instance;
        }
    }


    public virtual void OnRelease()
    {
        instance = default(T);
    }
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{

    private static T m_Instance = null;

    public static bool IsPersist { get; set; }

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                //m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                //if (m_Instance == null)
                //{
                string objName = "Singleton of " + typeof(T).ToString();
                var obj = GameObject.Find(objName);
                if (obj == null)
                    obj = new GameObject(objName);
                var getInst = obj.GetComponent<T>();
                if (getInst == null)
                    getInst = obj.AddComponent<T>();
                m_Instance = getInst;

                //    if (IsPersist)
                //    {
                DontDestroyOnLoad(m_Instance);
                //    }
                //}

            }
            return m_Instance;
        }
    }

    public virtual void OnRelease()
    {
        if (m_Instance != null)
        {
            Debug.Log("MonoSingleton.DestroySingleton Instance: " + m_Instance.GetType());

            var obj = m_Instance.gameObject;
            if (obj != null)
                Destroy(obj);
        }

        m_Instance = null;
    }

    private void Awake()
    {

        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
    }

    public virtual void Init() { }


    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}


