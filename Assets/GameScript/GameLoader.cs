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
        BindFGUI.BindAll();
        LoadData();
    }


    //load config data
    private void LoadData()
    {
        
        ConfigManager.LoadJsonInfos(() =>
        {
            Translator.Init();//load language table
            //TranslationHelper.translateStr = Translator.GetStr;
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
