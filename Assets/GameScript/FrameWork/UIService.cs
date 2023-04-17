using UniFramework.Singleton;

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
    static string redColor = "#ff5a5a";
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
}
