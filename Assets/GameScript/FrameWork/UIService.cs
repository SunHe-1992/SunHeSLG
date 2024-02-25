using UniFramework.Singleton;
using SunHeTBS;
using PackageShared;
using PackageBattle;
using YooAsset;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class UIService : ISingleton
{
    public static UIService Inst { get; private set; }
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<UIService>();
    }
    public void OnCreate(object createParam)
    {

    }

    public void OnDestroy()
    {

    }



    public void OnUpdate()
    {

    }

    static string greenColor = "#1baa6f";
    static string redColor = "#FF4F75";
    static string blueColor = "#3CD3F6";
    static string orangeColor = "#A56A3D";
    static string blackColor = "#000000";
    static string riceWhiteColor = "#FFF7EB";

    public static string ChangeGreen(string str)
    {
        return string.Format($"[color={greenColor}]{str}[/color]");
    }

    public static string ChangeRed(string str)
    {
        return string.Format($"[color={redColor}]{str}[/color]");
    }
    public static string ChangeBlue(string str)
    {
        return string.Format($"[color={blueColor}]{str}[/color]");
    }

    public static string DisplayValueStrBlue(int value, string name)
    {
        string prefix = "";
        if (value >= 0)
            prefix = " + ";
        string str = Translator.GetStr(name);
        string str2 = $"{str}:{prefix}{value}";
        return ChangeBlue(str2);
    }
    public static string DisplayValueStrRed(int value, string name)
    {
        string prefix = "";
        if (value >= 0)
            prefix = " + ";
        string str = Translator.GetStr(name);
        string str2 = $"{str}:{prefix}{value}";
        return ChangeRed(str2);
    }
    public static string DisplayValueStrAutoColor(int value, string name)
    {
        if (value >= 0)
            return DisplayValueStrBlue(value, name);
        else
            return DisplayValueStrRed(value, name);
    }

    public static string TileEffToString(int TileEffId)
    {

        var cfg = ConfigManager.table.TileEffect.GetOrDefault(TileEffId);
        if (cfg == null)
            return null;
        string str = "";
        if (cfg.Avoid != 0)
            str += UIService.DisplayValueStrAutoColor(cfg.Avoid, "Avoid") + ",";
        if (cfg.Heal != 0)
            str += UIService.DisplayValueStrAutoColor(cfg.Heal, "HealOnTurn") + ",";
        if (cfg.Damage != 0)
            str += UIService.DisplayValueStrAutoColor(cfg.Damage, "DamageOnTurn") + ",";
        if (cfg.Mov != 0)
            str += UIService.DisplayValueStrAutoColor(cfg.Mov, "Mov") + ",";
        if (cfg.DefFoe != 0)
            str += UIService.DisplayValueStrAutoColor(cfg.DefFoe, "DefResFoe") + ",";
        if (cfg.DefAlly != 0)
            str += UIService.DisplayValueStrAutoColor(cfg.DefAlly, "DefResAlly") + ",";
        if (cfg.UnBreakable)
            str += Translator.GetStr("Unbreakable") + ",";
        return str.Substring(0, str.Length - 1);
    }

    public void RefreshItemDetail(int itemId, UI_WeaponStats com)
    {
        var itemCfg = ConfigManager.table.Item.GetOrDefault(itemId);
        if (itemCfg == null)
        {
            return;
        }
        com.txt_des.text = itemCfg.Description;


    }
    Dictionary<string, Sprite> spriteCacheDic;
    public void LoadUnitySprite(string name, Image img)
    {
        Sprite sp = TryFindSprite(name);
        if (sp != null)
        {
            img.sprite = sp;
        }
        else
        {
            var handle = YooAssets.LoadAssetSync<Sprite>(name);
            handle.Completed += (loadObj) =>
            {
                if (loadObj.AssetObject != null)
                {
                    sp = loadObj.AssetObject as Sprite;
                    CacheSprite(name, sp);
                    img.sprite = sp;
                }
            };
        }
    }
    Sprite TryFindSprite(string name)
    {
        if (spriteCacheDic == null)
            spriteCacheDic = new Dictionary<string, Sprite>();
        if (spriteCacheDic.ContainsKey(name))
            return spriteCacheDic[name];
        return null;
    }
    void CacheSprite(string name, Sprite sp)
    {
        if (spriteCacheDic == null)
            spriteCacheDic = new Dictionary<string, Sprite>();
        if (sp != null)
            spriteCacheDic[name] = sp;
    }
}
