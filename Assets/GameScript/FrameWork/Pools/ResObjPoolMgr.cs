using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using YooAsset;

public class PoolResInfo
{
    public string resName;

    public PoolResInfo(string name)
    {
        this.resName = name;
    }
}

public class PrefabLoadData
{
    string AssetName;
    public GameObject PrefabObj;
    public AssetOperationHandle AssetOperationHandle;
    private List<UnityAction<string, GameObject>> ActionList;

    public PrefabLoadData(string assetName)
    {
        AssetName = assetName;
    }


    public void AppendAction(UnityAction<string, GameObject> action)
    {
        if (ActionList == null)
        {
            ActionList = new List<UnityAction<string, GameObject>>();
        }
        ActionList.Add(action);
    }

    public void InvokeActions()
    {
        if (ActionList == null)
        {
            return;
        }

        if (PrefabObj == null)
        {
            Debugger.LogError("No Prefab Object Loaded!");
            return;
        }

        int actionCount = ActionList.Count;
        for (int i = 0; i < actionCount; i++)
        {
            var action = ActionList[i];
            if (action != null)
            {
                action(AssetName, PrefabObj);
            }
        }
        ActionList.Clear();
    }

    public void Clear()
    {
        AssetName = "";

        PrefabObj = null;
    }


}

public class ResObjPoolMgr : MonoBehaviour
{
    /// <summary>
    /// 战斗中用到的资源池，每次退出战斗时其中的对象都会被销毁
    /// </summary>
    private static Dictionary<string, Pool> stageObjPool;
    private static Dictionary<string, Pool> hallObjPool;
    private static Dictionary<string, Object> cacheObjInfos; //缓存的数据

    // 当前正在加载的资源字典，用于控制同时加载的资源数量  
    private static Dictionary<string, float> preloadPoolNameDict;

    // 预加载资源队列，放到这个队列的就会被提前保证资源池中AssetObject先去加载
    private static Queue<string> PreloadResList;

    private static Dictionary<string, PrefabLoadData> prefabLoadingDict;
    private static List<string> HandledList; //用于缓存要加载的key的列表

    public static void RemovePoolNameFromPreload(string poolName)
    {
        preloadPoolNameDict.Remove(poolName);
    }

    public static ResObjPoolMgr Instance;

    static private Transform rootTrans;

    private bool stageResIsLoading = false;
    private static bool stageResLoadDone = true;

    //同时执行加载的最大数量
    public static int BUNDLE_LOAD_LIMIT = 8;

    void Awake()
    {
        Instance = this;

        stageObjPool = new Dictionary<string, Pool>();
        hallObjPool = new Dictionary<string, Pool>();
        cacheObjInfos = new Dictionary<string, Object>();
        preloadPoolNameDict = new Dictionary<string, float>();
        PreloadResList = new Queue<string>();

        prefabLoadingDict = new Dictionary<string, PrefabLoadData>();
        HandledList = new List<string>();

        stageResIsLoading = false;
        stageResLoadDone = true;
    }


    //清空所有战斗内的资源加载
    private static List<string> clearList; //清空的字典列表
    public static void ClearAllResourcesInFight()
    {
        if (clearList == null)
        {
            clearList = new List<string>();
        }
        else
        {
            clearList.Clear();
        }

        if (stageObjPool != null && stageObjPool.Count > 0)
        {
            foreach (var kv in stageObjPool)
            {
                var resname = kv.Key;
                Pool pool = kv.Value;

                clearList.Add(resname);
                pool.UnspawnAll();
            }

            int listCount = clearList.Count;
            for (int listIndex = 0; listIndex < listCount; listIndex++)
            {
                var resname = clearList[listIndex];

                if (stageObjPool.ContainsKey(resname))
                {
                    stageObjPool.Remove(resname);
                }
            }
        }
        else if (hallObjPool != null && hallObjPool.Count > 0)
        {
            foreach (var kv in hallObjPool)
            {
                var resname = kv.Key;
                Pool pool = kv.Value;

                clearList.Add(resname);
                pool.UnspawnAll();
            }

            int listCount = clearList.Count;
            for (int listIndex = 0; listIndex < listCount; listIndex++)
            {
                var resname = clearList[listIndex];

                if (hallObjPool.ContainsKey(resname))
                {
                    hallObjPool.Remove(resname);
                }
            }
        }
    }

    // 最近访问过的Pool，收缩到每个最多保留10个可用物体；最近三次都没再用过的，彻底释放掉
    private static List<string> shrinkList;
    public static void ShrinkStagePools(bool force = false)
    {
        if (clearList == null)
        {
            clearList = new List<string>();
        }
        else
        {
            clearList.Clear();
        }

        if (shrinkList == null)
        {
            shrinkList = new List<string>();
        }
        else
        {
            shrinkList.Clear();
        }

        if (stageObjPool != null && stageObjPool.Count > 0)
        {
            foreach (var kv in stageObjPool)
            {
                string poolName = kv.Key;
                Pool pool = kv.Value;

                pool.lastAccess++;
                if ((pool.lastAccess > 3 || force) && pool.GetTotalCount() == pool.GetAvailableCount())
                {
                    clearList.Add(poolName);
                }
                else
                {
                    if (pool.GetAvailableCount() > 10)
                    {
                        shrinkList.Add(poolName);
                    }
                }
            }

            int clearCount = clearList.Count;
            for (int i = 0; i < clearCount; i++)
            {
                string resname = clearList[i];
                if (stageObjPool.ContainsKey(resname))
                {
                    Pool pool = stageObjPool[resname];
                    pool.UnspawnAll();
                    stageObjPool.Remove(resname);
                }
            }

            int shrinkCount = shrinkList.Count;
            for (int i = 0; i < shrinkCount; i++)
            {
                string resname = shrinkList[i];
                if (stageObjPool.ContainsKey(resname))
                {
                    Pool pool = stageObjPool[resname];
                    pool.Shrink(10);
                }
            }

            Debugger.LogError("Cleared: " + clearCount + ", Shrinked: " + shrinkCount);
        }

        if (hallObjPool != null && hallObjPool.Count > 0)
        {
            foreach (var kv in hallObjPool)
            {
                string poolName = kv.Key;
                Pool pool = kv.Value;

                pool.lastAccess++;
                if ((pool.lastAccess > 3 || force) && pool.GetTotalCount() == pool.GetAvailableCount())
                {
                    clearList.Add(poolName);
                }
                else
                {
                    if (pool.GetAvailableCount() > 10)
                    {
                        shrinkList.Add(poolName);
                    }
                }
            }

            int clearCount = clearList.Count;
            for (int i = 0; i < clearCount; i++)
            {
                string resname = clearList[i];
                if (hallObjPool.ContainsKey(resname))
                {
                    Pool pool = hallObjPool[resname];
                    pool.UnspawnAll();
                    hallObjPool.Remove(resname);
                }
            }

            int shrinkCount = shrinkList.Count;
            for (int i = 0; i < shrinkCount; i++)
            {
                string resname = shrinkList[i];
                if (hallObjPool.ContainsKey(resname))
                {
                    Pool pool = hallObjPool[resname];
                    pool.Shrink(10);
                }
            }

            Debugger.LogError("Cleared: " + clearCount + ", Shrinked: " + shrinkCount);
        }
    }

    //是否加载完成了
    public static bool PreloadDone()
    {
        return stageResLoadDone;
    }


    static public void Unspawn(ObjectAgent objectAgent)
    {
        if (objectAgent == null)
        {
            Debugger.Log("Unspawn null gameobject");
            return;
        }

        Pool pool;
        string sName = objectAgent.ObjResName;


        bool bExist = stageObjPool.TryGetValue(sName, out pool); //寻找对应GameObject的池
        if (bExist)
        {
            pool.Unspawn(objectAgent);
        }
        else
        {
            bExist = hallObjPool.TryGetValue(sName, out pool); //寻找对应GameObject的池
            if (bExist)
                pool.Unspawn(objectAgent);

        }

    }

    private static bool Inited;
    public static void Init()
    {
        if (Inited)
        {
            return;
        }

        GameObject obj = new GameObject();
        obj.name = "ObjectPoolManager";
        obj.AddComponent<ResObjPoolMgr>();

        rootTrans = obj.transform;
        rootTrans.position = new Vector3(0, 100, 0);
        DontDestroyOnLoad(obj);

        Inited = true;
    }


    /// 从池中拿取一个GameObject
    static public void Take<TObject>(string name, CacheBack cacheBack) where TObject : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name))
        {
            Debugger.LogError("Invalid Name Give To Take, Path: " + name);
            return;
        }

        string poolName = name;
        Object info;

        var bExist = cacheObjInfos.TryGetValue(poolName, out info);//寻找对应GameObject的池

        if (cacheBack == null)
        {
            Debugger.LogError("回调有问题啊" + name);
            return;
        }

        if (bExist)
        {
            info = cacheObjInfos[poolName];

            cacheBack.CacheBackObj = info;
            if (cacheBack.customActionBackCache != null)
                cacheBack.customActionBackCache(cacheBack);
        }
        else
        {

            AssetOperationHandle assetOperationHandle;
            assetOperationHandle = YooAssets.LoadAssetSync<TObject>(name);


            assetOperationHandle.cacheBack = new CacheBack();
            assetOperationHandle.cacheBack.customParam = name;  //名称
            assetOperationHandle.cacheBack.customParamObj = cacheBack; //cache的数据
            assetOperationHandle.Completed += (aassetLoad) =>
            {
                if (aassetLoad.AssetObject == null)
                {
                    Debugger.LogError("获取的组件怎么会是空呢" + aassetLoad.cacheBack.customParam);
                    return;
                }

                var getInfo = aassetLoad.AssetObject;

                cacheObjInfos[aassetLoad.cacheBack.customParam] = getInfo;

                var CacheBack = (CacheBack)assetOperationHandle.cacheBack.customParamObj;
                CacheBack.CacheBackObj = getInfo;
                if (CacheBack.customActionBackCache != null)
                    CacheBack.customActionBackCache(CacheBack);
            };

        }
    }

    public static void PreloadAsset(string assetName, bool inFight, UnityAction<string> doneAction)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            Debugger.LogError("Invalid Asset To Preload: " + assetName);
            return;
        }

        string poolName = assetName;
        Pool pool;
        bool bExist;

        bExist = hallObjPool.TryGetValue(poolName, out pool);
        if (bExist)
        {
            doneAction?.Invoke(poolName);
            return;
        }

        bExist = stageObjPool.TryGetValue(poolName, out pool);
        if (bExist)
        {
            doneAction?.Invoke(poolName);
            return;
        }

        if (inFight)
        {
            pool = new Pool(rootTrans, poolName);
            stageObjPool.Add(poolName, pool);  //放入StageObjPool中，战斗结束之后会清理掉

        }
        else
        {
            pool = new Pool(rootTrans, poolName);
            hallObjPool.Add(poolName, pool);  //放入StageObjPool中，战斗结束之后会清理掉
        }

        pool.StartPreload(doneAction);

    }


    /// 从池中拿取一个GameObject
    /// ObjectAgent是池子，只适用于gameobject
    static public ObjectAgent Take(string name, bool inFight)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debugger.LogError("Invalid Name Give To Take, Path: " + name);
            return null;
        }

        Pool pool;
        bool bExist;

        ObjectAgent agent = null;

        string poolName = name;

        if (inFight)
        {
            bExist = stageObjPool.TryGetValue(poolName, out pool);//寻找对应GameObject的池

            if (bExist)
            {
                agent = pool.Spawn();
            }
            else
            {
                pool = new Pool(rootTrans, name);

                stageObjPool.Add(poolName, pool);  //放入StageObjPool中，战斗结束之后会清理掉


                agent = pool.Spawn();
            }

            if (agent != null)
            {
                agent.SetActive(false);
                agent.unspawned = false;
            }
        }
        else
        {
            bExist = hallObjPool.TryGetValue(poolName, out pool);//寻找对应GameObject的池

            if (bExist)
            {
                agent = pool.Spawn();
            }
            else
            {
                pool = new Pool(rootTrans, name);

                hallObjPool.Add(poolName, pool);  //放入StageObjPool中，战斗结束之后会清理掉

                agent = pool.Spawn();
            }

            if (agent != null)
            {
                agent.SetActive(false);
                agent.unspawned = false;
            }

        }


        return agent;
    }


    //读取资源名
    public static void LoadGameObject(string assetPath, UnityAction<string, GameObject> action)
    {
        if (prefabLoadingDict.ContainsKey(assetPath))
        {
            var loadingData = prefabLoadingDict[assetPath];
            loadingData.AppendAction(action);
        }
        else
        {
            PrefabLoadData loadingData = new PrefabLoadData(assetPath);
            loadingData.AppendAction(action);

            prefabLoadingDict[assetPath] = loadingData;
        }


        AssetOperationHandle assetOperationHandle;
        assetOperationHandle = YooAssets.LoadAssetSync<GameObject>(assetPath);


        assetOperationHandle.cacheBack = new CacheBack();
        assetOperationHandle.cacheBack.customParam = assetPath;
        assetOperationHandle.Completed += (aassetLoad) =>
        {
            if (aassetLoad.AssetObject == null)
            {
                Debugger.LogError("获取的组件怎么会是空呢" + aassetLoad.cacheBack.customParam);
                return;
            }

            GameObject prefabObj = (GameObject)aassetLoad.AssetObject;

            if (prefabLoadingDict.ContainsKey(aassetLoad.cacheBack.customParam))
            {
                PrefabLoadData loadData = prefabLoadingDict[aassetLoad.cacheBack.customParam];
                loadData.PrefabObj = prefabObj;
                loadData.AssetOperationHandle = aassetLoad;
            }
            else
            {
                Debugger.LogError("No Prefab Loading Dict For Asset Path: " + aassetLoad.cacheBack.customParam);
            }
        };
    }


    // 自己就是个MonoBehavior，自己驱动自己的静态更新函数
    public void Update()
    {
        FrameUpdate();
    }

    public static void FrameUpdate()
    {
        if (!Inited)
        {
            return;
        }

        if (prefabLoadingDict.Count <= 0)
        {
            return;
        }

        HandledList.Clear();
        bool haveHandled = false;
        foreach (var kv in prefabLoadingDict)
        {
            string keyStr = kv.Key;
            PrefabLoadData loadData = kv.Value;
            if (loadData.PrefabObj != null)
            {
                HandledList.Add(keyStr);
                haveHandled = true;
            }
        }

        if (haveHandled)
        {
            int handledCount = HandledList.Count;
            for (int i = 0; i < handledCount; i++)
            {
                string key = HandledList[i];

                if (!prefabLoadingDict.ContainsKey(key))
                {
                    continue;
                }

                var loadData = prefabLoadingDict[key];
                loadData.InvokeActions();

                loadData.Clear();
                prefabLoadingDict.Remove(key);
            }
        }

    }

}

//[System.Serializable]
public class Pool
{
    public int ID = -1;
    public int lastAccess = -1;        //记录空闲时长

    public string ResName { get; private set; }
    private Transform parentTrans;     //场景切换时不销毁的父物体

    private string assetName;
    public GameObject PrefabObj;

    private int totalObjCount = 0;
    /// <summary>
    /// 加载资源中
    /// </summary>
    public bool isLoading;

    public delegate void GameObjectLoadedHandler(Object obj);

    private List<ObjectAgent> available = new List<ObjectAgent>();
    private List<ObjectAgent> allObject = new List<ObjectAgent>();


    public Pool()
    { }

    //name是相对于gameres下的目录路径
    public Pool(Transform pTrans, string name)
    {
        this.parentTrans = pTrans;
        ResName = name;
        lastAccess = 0;
    }

    public void StartPreload(UnityAction<string> loadDoneAction = null)
    {
        isLoading = true;

        string fullPathName = ResName;

        ResObjPoolMgr.LoadGameObject(fullPathName, (name, obj) =>
        {
            isLoading = false;
            if (obj == null)
            {
                Debugger.LogError("No Object " + fullPathName + " Found.");
            }
            else
            {
                assetName = name;
                PrefabObj = obj;

            }

            loadDoneAction?.Invoke(fullPathName);

            for (int i = this.allObject.Count - 1; i >= 0; i--)
            {
                var agent = allObject[i];
                if (agent.pendingLoadResource)
                {
                    agent.pendingLoadResource = false;

                    agent.OnPrefabLoaded(PrefabObj);
                    agent.IsLoaded = true;
                }
            }
        });

    }


    private void ClearEmptyObject()
    {
        int objCount = available.Count;
        if (objCount <= 0) return;

        for (int index = objCount - 1; index >= 0; index--)
        {
            ObjectAgent spawnObj = available[index];
            if (spawnObj == null)
            {
                Debugger.LogError("Cleanup empty spawn obj from pool: " + this.ResName);
                available.RemoveAt(index);
                totalObjCount--;
            }
        }
    }

    //创建一个新的对象实例
    public ObjectAgent Spawn()
    {
        ObjectAgent spawnAgent = null;

        lastAccess = 0;
        ClearEmptyObject();

        // 找有没有可用的
        if (available.Count > 0)
        {
            int lastIndex = available.Count - 1;

            spawnAgent = available[lastIndex];
            available.RemoveAt(lastIndex);

            if (spawnAgent == null)
            {
                Debugger.LogError("no gameobject spawned : " + this.ResName + ", Available Count: " + available.Count);
            }
        }
        else
        {
            GameObject spawnObj = new GameObject();
            spawnAgent = spawnObj.AddComponent<ObjectAgent>();

            if (this.parentTrans != null)
            {
                spawnObj.transform.SetParent(this.parentTrans);
            }

            allObject.Add(spawnAgent);

            spawnObj.name = this.ResName;
            spawnAgent.ObjResName = this.ResName;

            if (PrefabObj != null)
            {
                // 预制已经加载，创建实例
                spawnAgent.CreateObjectInst<GameObject>(PrefabObj);
            }
            else
            {
                // 预制还没加载出来，先记录这个Agent，启动Prefab的加载
                spawnAgent.pendingLoadResource = true;

                if (isLoading == false)
                {

                    this.StartPreload((fullName) =>
                    {
                        ResObjPoolMgr.RemovePoolNameFromPreload(fullName);
                    });
                }
            }

            totalObjCount += 1;
        }

        spawnAgent.transform.rotation = Quaternion.identity;
        spawnAgent.transform.parent = null;
        spawnAgent.SetOnLoadAction(null);

        return spawnAgent;
    }


    public void Unspawn(ObjectAgent agent)
    {
        if (agent == null || agent.gameObject == null)
        {
            Debugger.LogWarning("Given Object Agent to Unspawn is NULL!");
            return;
        }

        if (agent.unspawned)
        {
            //Debugger.LogWarning("Agent Duplex Unspawn Detected: " + agent.ObjResName);
            return;
        }

        if (agent != null)
        {
            if (available.Count > 60)
            {
                //空闲的够多了，直接释放掉
                allObject.Remove(agent);
                if (agent.BindingGameObject != null)
                {
                    GameObject.Destroy(agent.BindingGameObject);
                    agent.BindingGameObject = null;
                }

                GameObject.Destroy(agent.gameObject);
                return;
            }

            agent.SetOnLoadAction(null);
            if (agent.gameObject == null)
            {
                return;
            }

            if (agent.BindingGameObject != null)
                available.Add(agent);

            Transform agentTrans = agent.transform;

            // 一些特效使用后会被挂在目标对象身上，回收时需要把其父物体也重置回来
            if (parentTrans != null)
            {
                agentTrans.SetParent(parentTrans);
                agentTrans.SetPositionAndRotation(parentTrans.position, parentTrans.rotation);
            }
            else
            {
                agentTrans.SetParent(null);
                agentTrans.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            agentTrans.localScale = Vector3.one;

            agent.gameObject.SetActive(false);
            agent.Unspawned();
        }
    }

    //释放所有的资源
    public void UnspawnAll()
    {
        var listCount = allObject.Count;
        for (int listIndex = 0; listIndex < listCount; listIndex++)
        {
            var obj = allObject[listIndex];

            if (obj != null)
            {
                if (obj.BindingGameObject != null)
                {
                    GameObject.Destroy(obj.BindingGameObject);
                    obj.BindingGameObject = null;
                }
                GameObject.Destroy(obj.gameObject);
            }
        }

        available.Clear();
        allObject.Clear();

        PrefabObj = null;
        parentTrans = null;

        totalObjCount = 0;
    }


    public int GetTotalCount()
    {
        return totalObjCount;
    }

    public int GetAvailableCount()
    {
        if (this.available == null)
        {
            return 0;
        }

        return available.Count;
    }

    public void Shrink(int maxNo)
    {
        // 把多余的物体释放掉
        int availableCount = GetAvailableCount();
        if (availableCount <= maxNo)
        {
            return;
        }

        for (int i = availableCount - 1; i >= maxNo; i--)
        {
            ObjectAgent oa = available[i];

            if (oa.BindingGameObject != null)
            {
                GameObject.Destroy(oa.BindingGameObject);
                oa.BindingGameObject = null;
            }

            available.RemoveAt(i);
        }

        int totalCount = allObject.Count;
        for (int i = totalCount - 1; i >= 0; i--)
        {
            // 第二轮清理掉ObjectAgent
            ObjectAgent oa = allObject[i];
            if (oa == null)
            {
                allObject.RemoveAt(i);
                continue;
            }

            if (oa.BindingGameObject == null)
            {
                allObject.RemoveAt(i);
                GameObject.Destroy(oa.gameObject);
            }
        }
    }
}


public class ObjectAgent : MonoBehaviour
{
    //private Pool Pool;              // 反向索引，所属的对象池
    public bool IsLoaded = false;
    /// <summary>
    /// 已经回收标记
    /// </summary>
    public bool unspawned = false;

    public string ObjResName;       //索引Pool时使用的资源名称
    public Vector3 CustomVector; //客户端参数

    public GameObject BindingGameObject = null;  // 绑定的游戏物体



    //-------------客户端的存储的数据
    public UnityAction customAction;
    public UnityAction customAction2;
    public UnityAction<GameObject> customActionBackObj;

    public string customParam;
    public string customParam2;

    public object customParamObj;
    public object customParamObj2;
    public object customParamObj3;
    public object customParamObj4;
    public object customParamObj5;

    public int customIntParam;
    public Dictionary<int, int> customValues;
    public Dictionary<string, int> customValues_ByString;
    public Dictionary<string, string> customValues_ByString2;



    //agent拿资源,pool没有加载资源 则等待, 直到pool加载资源后遍历此字段true的agent 进行回调操作
    /// <summary>
    /// 等待pool加载资源中
    /// </summary>
    public bool pendingLoadResource;

    public void CreateObjectInst<TObject>(GameObject prefabObj) where TObject : UnityEngine.Object
    {
        if (prefabObj == null)
        {
            Debugger.LogError("Invalid Prefab Given To Create Object: " + ObjResName);
            return;
        }

        if (this.BindingGameObject != null)
        {
            Debugger.LogError("Not Expected Situation, Binding Game Object Alread Exist: " + ObjResName);
            Object.Destroy(this.BindingGameObject);
        }

        this.pendingLoadResource = false;
        this.BindingGameObject = GameObject.Instantiate(prefabObj);
        if (BindingGameObject != null)
        {
            Transform bindingTrans = this.BindingGameObject.transform;

            bindingTrans.SetParent(this.transform);

            bindingTrans.localPosition = Vector3.zero;
            bindingTrans.localRotation = Quaternion.identity;

#if UNITY_EDITOR
            GameUtilFunc.FixShaderInEditor(this.BindingGameObject);
#endif

            IsLoaded = true;
        }

        if (!this.unspawned)
        {
            if (onLoadAction != null)
            {
                var act = onLoadAction;
                onLoadAction = null;
                act(this.BindingGameObject);
            }
        }
        else
        {
            Debugger.LogError("Object Agent Already Unspawned After Instantiated: " + ObjResName);
        }


    }

    internal void OnPrefabLoaded(GameObject prefabObj)
    {
        try
        {
            if (prefabObj == null)
            {
                if (!this.unspawned && onLoadAction != null)
                {
                    var act = onLoadAction;
                    onLoadAction = null;
                    act(null);
                }
                return;
            }

            if (this.IsLoaded == false && this.BindingGameObject == null)//如果之前没有资源物体才复制一个
            {
                this.BindingGameObject = GameObject.Instantiate(prefabObj);
                if (BindingGameObject != null)
                {
                    IsLoaded = true;
                    Transform bindingTrans = BindingGameObject.transform;

                    if (this.gameObject != null)
                    {
                        bindingTrans.SetParent(this.gameObject.transform);
                    }
                    bindingTrans.localPosition = Vector3.zero;
                    bindingTrans.localRotation = Quaternion.identity;

#if UNITY_EDITOR
                    GameUtilFunc.FixShaderInEditor(this.BindingGameObject);
#endif
                }
                else
                {
                    Debugger.LogError("实例化BindingGameObject失败：" + this.ObjResName);
                }

            }

            if (!this.unspawned && onLoadAction != null)
            {
                var act = onLoadAction;
                onLoadAction = null;
                act(this.BindingGameObject);

            }
        }
        catch (System.Exception e)
        {
            if (prefabObj != null)
            {
                Debugger.LogError("When Handle Loaded Prefab: " + prefabObj.name);
            }
            Debugger.LogError("Exception in OnPrefabLoaded: " + e.ToString());
            if (!this.unspawned && onLoadAction != null)
            {
                var act = onLoadAction;
                onLoadAction = null;
                act(null);
            }
        }
    }

    public void SetActive(bool flag)
    {
        if (this.gameObject.activeSelf != flag)
        {
            this.gameObject.SetActive(flag);
        }

        if (this.BindingGameObject != null)
        {

            var obj = ((GameObject)(this.BindingGameObject));
            if (obj.activeSelf != flag)
            {
                obj.SetActive(flag);
            }


        }
    }

    public bool activeInHierarchy()
    {
        return this.gameObject.activeSelf;
    }

    UnityEngine.Events.UnityAction<GameObject> onLoadAction;

    UnityAction onEnableAction = null;
    UnityAction OnDisableAction = null;
    UnityAction unSpawnedAction = null;

    public void SetOnLoadAction(UnityEngine.Events.UnityAction<GameObject> onload)
    {
        onEnableAction = null;
        OnDisableAction = null;
        unSpawnedAction = null;

        if (this.IsLoaded && onload != null)
        {
            if (this.BindingGameObject == null)
            {
                // 提示一下有加载失败的资源
                Debugger.LogError("No GameObject Loaded By: " + this.ObjResName);
            }

            onLoadAction = null;
            onload(this.BindingGameObject);
            return;
        }
        onLoadAction = onload;
    }


    public void SetEnableAction(UnityAction onEnableAction_ = null, UnityAction onDisableAction_ = null, UnityAction unSpawnedAction_ = null)
    {
        onEnableAction = onEnableAction_;
        OnDisableAction = onDisableAction_;
        unSpawnedAction = unSpawnedAction_;
    }

    void OnEnable()
    {
        if (onEnableAction != null)
            onEnableAction();
    }



    void OnDisable()
    {
        if (OnDisableAction != null)
            OnDisableAction();
    }

    public void Unspawned()
    {
        if (unSpawnedAction != null)
            unSpawnedAction();

        unspawned = true;
        onEnableAction = null;
        OnDisableAction = null;
        unSpawnedAction = null;
    }

    // 销毁的时候，断开与游戏物体的连接
    public void OnDestroy()
    {
        //Pool = null;
        BindingGameObject = null;

        onEnableAction = null;
        OnDisableAction = null;
        unSpawnedAction = null;
    }
}
