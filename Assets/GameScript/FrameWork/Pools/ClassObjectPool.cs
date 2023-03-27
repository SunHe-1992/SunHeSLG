using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//todo 对象池支持非泛型
public class ClassPoolMgr
{
    /// <summary>
    /// 管理所有类对象池的Dic
    /// </summary>
    private static Dictionary<Type, IPool> m_ClassPoolDic;
    private static List<IPool> m_ClassPoolList;

    private static bool m_CanCheck = true;

    public static void InitClassPool()
    {
        m_ClassPoolDic = new Dictionary<Type, IPool>();
        m_ClassPoolList = new List<IPool>();
    }


    /// <summary>
    /// 创建类对象池，创建完成以后可以保存ClassObjectPool<T>,然后可以调用swapn和recyle创建和回收类对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    private static ClassObjectPool<T> GetClassPool<T>(int maxCount = 1) where T : class, IRecycle, new()
    {
        Type type = typeof(T);
        IPool obj = null;
        if (!m_ClassPoolDic.TryGetValue(type, out obj) || obj == null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
            m_ClassPoolDic.Add(type, newPool);
            m_ClassPoolList.Add(newPool);
            return newPool;
        }
        return obj as ClassObjectPool<T>;
    }

    /// <summary>
    /// 从池中获取对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetObject<T>() where T : class, IRecycle, new()
    {
        ClassObjectPool<T> pool = GetClassPool<T>();
        if (pool == null)
        {
            Debugger.LogError("获取classpool失败：" + typeof(T).ToString());
            return null;
        }
        return pool.Spawn(true);
    }
    /// <summary>
    /// 回收对象到池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool RecycleObject<T>(T obj) where T : class, IRecycle, new()
    {
        ClassObjectPool<T> pool = GetClassPool<T>();
        if (pool == null)
        {
            Debugger.LogError("获取classpool失败：" + typeof(T).ToString());
            return false;
        }
        return pool.Recycle(obj);
    }

    public static ClassObjectPool<T> TestPool<T>() where T : class, IRecycle, new()
    {
        ClassObjectPool<T> pool = GetClassPool<T>();
        return pool;
    }

    public static void CheckUnRecyclePool()
    {
        if (m_CanCheck)
        {
            for (int i = 0; i < m_ClassPoolList.Count; i++)
            {
                int noRecycleCount = m_ClassPoolList[i].GetNoRecycleCount();
                if (noRecycleCount > 0)
                {
                    string className = m_ClassPoolList[i].GetType().ToString();
                    Debugger.LogError("类对象池有对象没有回收，检查是否忘记回收：" + className + "-----NoRecycleCount:" + noRecycleCount);
                    m_ClassPoolList[i].CheckNoRecycleList();
                }
            }
        }
    }
    public static void CheckOnRecyclePool()
    {
        //    foreach (KeyValuePair<Type, IPool> kv in m_ClassPoolDic)
        //{
        //    Debugger.LogError("已回收类型：" + kv.Key.ToString() + "-----RecycleCount:" + kv.Value.GetRecycleCount());
        //}
    }

}

public class ClassObjectPool<T> : IPool where T : class, IRecycle, new()
{
    /// <summary>
    /// 池
    /// </summary>
    private Stack<T> m_Pool = new Stack<T>();

    private List<T> m_NoRecycleList = new List<T>();
    /// <summary>
    /// 最大对象个数，m_MaxCount小于等于0表示不限制个数
    /// </summary>
    private int m_MaxCount = 5;
    /// <summary>
    /// 没有回收的对象个数
    /// </summary>
    private int m_NoRecycleCount = 0;
    /// <summary>
    /// 默认池大小为5
    /// </summary>
    /// <param name="maxCount"></param>
    public ClassObjectPool(int maxCount = 5)
    {
        m_MaxCount = maxCount;
        for (int i = 0; i < m_MaxCount; i++)
        {
            m_Pool.Push(new T());
        }
    }
    /// <summary>
    /// 从池里面取类对象，如果creatIfEmpty为true，无论空池或者从池里取出来的是null，都会返回一个不为空的对象
    /// 如果creatIfEmpty为false，从池中取出来的有可能是null
    /// </summary>
    /// <param name="creatIfEmpty"></param>
    /// <returns></returns>
    public T Spawn(bool creatIfEmpty)
    {
        T obj = null;
        if (m_Pool.Count > 0)
        {
            obj = m_Pool.Pop();
            if (obj == null)
            {
                if (creatIfEmpty)
                {
                    obj = new T();
                }
            }
            m_NoRecycleCount++;
        }
        else
        {
            if (creatIfEmpty)
            {
                obj = new T();
                m_NoRecycleCount++;
            }
        }

        obj.OnCreate();

#if UNITY_EDITOR
        {
            m_NoRecycleList.Add(obj);
        }
#endif
        return obj;
    }
    /// <summary>
    /// 回收类对象
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool Recycle(T obj)
    {
        if (obj == null)
        {
            return false;
        }

        m_NoRecycleCount--;
        obj.Recycle();
        m_Pool.Push(obj);
        //m_NoRecycleList.Remove(obj);
        return true;
    }

    public int GetNoRecycleCount()
    {
        return m_NoRecycleList.Count;
    }

    public int GetRecycleCount()
    {
        return m_Pool.Count;
    }

    public void CheckNoRecycleList()
    {
        int count = m_NoRecycleList.Count;

        for (int i = 0; i < count; i++)
        {
            IRecycle data = m_NoRecycleList[i];
            //Debugger.LogError(uicontrolData.GetDebugInfo());
        }

        m_NoRecycleList.Clear();
        m_NoRecycleCount = 0;
    }
}

public interface IRecycle
{
    void OnCreate();
    void Recycle();
    string GetDebugInfo();
}

public interface IPool
{
    int GetNoRecycleCount();
    int GetRecycleCount();
    void CheckNoRecycleList();
}


