using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        BindFGUI.BindAll();//fairy code bind 
        LoadData();//load json configs
        ResObjPoolMgr.Init();//object pool initialize
        FUIManager.ReSetBundle();//FUIManager initialize
        FUIManager.Instance.Init();

        //show test ui
        FUIManager.Instance.ShowUI<UIPage_Debug>(FUIDef.FWindow.TestUI);
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
}
