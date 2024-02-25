using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using YooAsset;
using UnityEngine.Events;
using UniFramework.Singleton;
public class ConfigManager : ISingleton
{

    public static ConfigManager Inst { get; private set; }
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<ConfigManager>();
    }
    public void OnCreate(object createParam)
    {
    }

    public void OnUpdate()
    {
    }

    public void OnDestroy()
    {
        table = null;
    }
    /// <summary>
    /// 
    /// </summary>
    public static cfg.Tables table;
    public static void ReadRawTables()
    {
        //editor read json files
#if UNITY_EDITOR
        table = new cfg.Tables(LoadJsonBufInfo);
        //table = new cfg.Tables(LoadByteBuf);
#else
         table = new cfg.Tables(LoadJsonBufInfo);
#endif
    }


    /// <summary>
    /// ��ȡjson�������
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static JSONNode LoadJsonBufInfo(string file)
    {
        if (allInfos.ContainsKey(file))
            return allInfos[file];
        else
        {
            Debugger.LogError("�⼸���ļ���û�м������ݣ���" + file);
            return null;
        }
    }

    /// <summary>
    /// ��ȡjson�������
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    static Dictionary<string, JSONNode> allInfos = new Dictionary<string, JSONNode>();
    static UnityAction unityActionBack;
    static int allNums = 0;
    public static void LoadJsonInfos(UnityAction unityAction)
    {
        allInfos.Clear();

        unityActionBack = unityAction;

        //��ȡdata������Щ����Ҫ�Լ�д������
        var allInfosConfig = YooAssets.GetAssetInfos("config");
        allNums = allInfosConfig.Length;
        foreach (var info in allInfosConfig)
        {
            var path = info.AssetPath;
            var GetRealName = path.Split("/");
            path = GetRealName[GetRealName.Length - 1];

            GetRealName = path.Split(".");
            path = GetRealName[0];
            LoadInfo(path);
        }
    }

    static void CheckLoadAll()
    {
        allNums = allNums - 1;
        if (allNums == 0)
        {
            ReadRawTables();
            if (unityActionBack != null)
                unityActionBack();
        }
    }

    public static void LoadInfo(string name)
    {

        string tableDataFile = $"Config/{name}";

        AssetHandle assetOperationHandle;

        assetOperationHandle = YooAssets.LoadAssetSync<TextAsset>(tableDataFile);

        assetOperationHandle.Completed += (assetLoad) =>
        {
            if (assetLoad == null)
            {
                CheckLoadAll();
                return;
            }

            var objText = (TextAsset)assetLoad.AssetObject;
            allInfos.Add(name, JSON.Parse(objText.text));
            CheckLoadAll();
        };

    }

}
