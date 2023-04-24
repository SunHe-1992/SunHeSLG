using System.Collections.Generic;
using UnityEngine;

public class Translator
{

    static Translator instance_;
    public static Translator Instance
    {
        get
        {
            if (instance_ == null)
                instance_ = new Translator();
            return instance_;
        }
    }

    public static bool Initialized
    {
        get;
        private set;
    }


    public static Dictionary<SystemLanguage, string> langNameDict = null;
    public static SystemLanguage currLanguage = SystemLanguage.English;
    public static string defaultLangName = "EN";



    public static SystemLanguage CurrLanguage
    {
        get
        {
            return currLanguage;
        }
    }


    public static string GetStr(string key)
    {
        return getText(key);
    }

    public static void Init()
    {

        Initialized = false;
        LoadCurrLanguage();
        LoadTextDict();
    }

    // TODO： 获取当前系统语言，或者玩家选择的语言
    public static void LoadCurrLanguage()
    {
        currLanguage = SystemLanguage.English;
        defaultLangName = "EN";

        //currLanguage = SystemLanguage.ChineseSimplified;
        //defaultLangName = "zh-CN";
    }

    public static void InitLangNameDict()
    {
        langNameDict = new Dictionary<SystemLanguage, string>();

        langNameDict[SystemLanguage.ChineseSimplified] = "zh-CN";
        langNameDict[SystemLanguage.Chinese] = "zh-CN";
    }

    public static void AddLangName(SystemLanguage language, string name)
    {
        langNameDict[language] = name;
    }

    public static void SetDefault(SystemLanguage system, string name)
    {
        currLanguage = system;
        defaultLangName = name;
    }

    public static string getCurrLanguageName(SystemLanguage lang)
    {
        if (langNameDict == null)
        {
            InitLangNameDict();
        }

        string languageFileName = defaultLangName;
        if (langNameDict.ContainsKey(lang))
        {
            languageFileName = langNameDict[lang];
        }

        return languageFileName;
    }

    static Dictionary<string, Dictionary<string, string>> allInfos; //表格所有数据存储

    public static void LoadTextDict()
    {
        //表加载了已经，开始转换内容
        if (allInfos == null)
            allInfos = new Dictionary<string, Dictionary<string, string>>();
        else
            allInfos.Clear();

        var tbs = ConfigManager.table.TbLanguage.DataList;
        foreach (var tbInfo in tbs)
        {
            allInfos.Add(tbInfo.ID, new Dictionary<string, string>());
            foreach (var infoOneLan in tbInfo.Lans)
            {
                var stringValue = infoOneLan.Value;
                if (!string.IsNullOrEmpty(stringValue) && stringValue.Contains
                     ("\\n"))
                {
                    stringValue = stringValue.Replace("\\n", "\n");
                }

                allInfos[tbInfo.ID].Add(infoOneLan.Key, stringValue);
            }
        }

        Initialized = true;
    }


    //获取文本的内容
    public static string getText(string key)
    {
        string result = "";
        if (string.IsNullOrEmpty(key))
        {
            return result;
        }


        if (!allInfos.ContainsKey(key))
        {
            if (key.Contains("\\n"))
            {
                key = key.Replace("\\n", "\n");
            }
            return key;
        }



        if (!allInfos[key].ContainsKey(defaultLangName))
        {
            Debugger.LogError("多语言没有这类语言的文字");
            return key;
        }
        return allInfos[key][defaultLangName];
    }

    // 文件资源加载之后的处理
    private static void PostProcessLangText(string langText)
    {
        if (langText == null)
        {
            Debugger.LogError("No Language Text Loaded!");
            return;
        }

        string[] separators = new string[] { "\n" };
        string[] allLines = langText.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        //ax.Unload (false);

        char[] blanks = new char[] { ' ', '\r', '\n' };

        int lineno = 0;

        foreach (string line in allLines)
        {
            try
            {
                int splitpos = line.IndexOf(" = ");
                if (splitpos < 1)
                    continue;

                string kv = line.TrimEnd(blanks);
                string key = kv.Substring(0, splitpos).Trim(blanks);
                string value = kv.Substring(splitpos + 3).Trim(blanks);

                value = value.Replace("\\n", "\n");

                // textDict[key] = RTL(value);
                lineno++;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(string.Format("Parse language {0} error:[{1}]{2}", currLanguage, lineno, line), ex.InnerException);
            }
        }
    }

    static string RTL(string source)
    {
        if (currLanguage == SystemLanguage.Arabic || currLanguage == SystemLanguage.Hebrew)
        {
            Debugger.LogError("Leak Arabic Support Plugins!");
        }
        return source;
    }
}
