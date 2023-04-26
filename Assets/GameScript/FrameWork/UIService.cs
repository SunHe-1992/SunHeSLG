using UniFramework.Singleton;
using SunHeTBS;
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

    public void OnFixedUpdate()
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
}
