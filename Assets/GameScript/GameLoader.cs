using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using UnityEngine.UIElements;
using YooAsset;
using UniFramework.Pooling;
public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance = null;

    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitEnv()
    {
        UniPooling.Initalize();
        SunHeTBS.BattleDriver.UniSpawner = UniPooling.CreateSpawner("DefaultPackage");
        BindFGUI.BindAll();//fairy code bind 
        LoadData();//load json configs

        LoadFontRes();
        FUIManager.ReSetBundle();//FUIManager initialize
        FUIManager.Instance.Init();


    }


    //load config data
    private void LoadData()
    {

        ConfigManager.LoadJsonInfos(() =>
        {
            Translator.Init();//load language table
            FairyGUI.TranslationHelper.translateStr = Translator.GetStr;
            //ConfigInited = true;
            //TestAfterLoadConfig();
        });
    }
    //void TestAfterLoadConfig()
    //{
    //    string testStr = Translator.GetStr("T-test");
    //    Debugger.Log("test str " + testStr);
    //}

    UnityEngine.Font loadFontObj;
    private void LoadFontRes()
    {
        var loadHeiti = YooAssets.LoadAssetAsync<UnityEngine.Font>("UIFont/OPPOSans-M");
        loadFontObj = null;
        #region 字体加载
        loadHeiti.Completed += (loadDown) =>
        {
            loadFontObj = (loadDown.AssetObject as UnityEngine.Font);

            LoadFontDone();
        };
        #endregion
    }
    int needDoneAllNum;
    void LoadFontDone()
    {
        string fontNameStr = "OPPOSans-M";
        DynamicFont fontHeiti = new DynamicFont(fontNameStr, loadFontObj);
        FontManager.RegisterFont(fontHeiti, fontNameStr);

        needDoneAllNum = 2;

        FUIManager.Instance.IncPackageReference("PackageShared");
        FUIManager.Instance.PreAddPackage(FUIDef.FPackage.PackageShared.ToString(), loadCommonDone);
        FUIManager.Instance.IncPackageReference("PackageDebug");
        FUIManager.Instance.PreAddPackage(FUIDef.FPackage.PackageDebug.ToString(), loadCommonDone);
    }

    void loadCommonDone()
    {
        needDoneAllNum = needDoneAllNum - 1;

        if (needDoneAllNum == 0)
        {
            if (PatchWindow.Inst)
            {
                Destroy(PatchWindow.Inst);
            }
            //show test ui
            FUIManager.Instance.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
            DontDestroyOnLoad(StageCamera.main);
        }
    }
}
