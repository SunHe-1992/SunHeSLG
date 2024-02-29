using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using UnityEngine.UIElements;
using UniFramework.Singleton;
using UniFramework.Animation;
using UniFramework.Pooling;
using UniFramework.Tween;
//using UniFramework.Window;
using YooAsset;
using SunHeTBS;
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

        UniSingleton.Initialize();

        UniTween.Initalize();

        SunHeTBS.BattleDriver.UniSpawner = UniPooling.CreateSpawner("DefaultPackage");
        //init Uni singletons
        /*fire emblem demo */
        BattleDriver.Init();
        BLogic.Init();
        TBSMapService.Init();
        //monopoly game singletons
        MonopolyDriver.Init();
        MLogic.Init();

        //framework
        FUIManager.Init();
        ConfigManager.Init();
        InputManager.Init();
        UIAnimationService.Init();
        UIService.Init();

        BindFGUI.BindAll();//fairy code bind 
        LoadData();//load json configs

        FUIManager.ReSetBundle();//FUIManager initialize
        LoadFontRes();

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

        FUIManager.Inst.IncPackageReference("PackageShared");
        FUIManager.Inst.PreAddPackage(FUIDef.FPackage.PackageShared.ToString(), loadCommonDone);
        FUIManager.Inst.IncPackageReference("PackageDebug");
        FUIManager.Inst.PreAddPackage(FUIDef.FPackage.PackageDebug.ToString(), loadCommonDone);
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
            FUIManager.Inst.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
            DontDestroyOnLoad(StageCamera.main);
        }
    }
}
