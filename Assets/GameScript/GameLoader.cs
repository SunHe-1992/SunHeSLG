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
        //BindFGUI.BindAll();
        LoadData();
    }


    //加载表格
    private void LoadData()
    {

        ConfigManager.LoadJsonInfos(() =>
        {
            //Translator.Init();
            //TranslationHelper.translateStr = Translator.GetStr;
            //ConfigInited = true;
            //TestAfterLoadConfig();
        });
    }
    //void TestAfterLoadConfig()
    //{
    //    var tbconst = ConfigManager.table.TbConst;
    //    var tbLang= ConfigManager.table.TbLanguage;
    //}
}
