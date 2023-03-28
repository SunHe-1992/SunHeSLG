using System.Collections.Generic;
public class FUIDef
{
    /// <summary>
    /// window names
    /// </summary>
    public enum FWindow
    {
        TestUI,
        SamplePage,
    }
    /// <summary>
    /// package names
    /// </summary>
    public enum FPackage
    {
        PackageDebug,

    }
    /// <summary>
    /// dic : key=window name, value=package name
    /// </summary>
    public static Dictionary<FWindow, FPackage> windowUIpair = new Dictionary<FWindow, FPackage>()
    {
        {FWindow.TestUI, FPackage.PackageDebug},
        {FWindow.SamplePage, FPackage.PackageDebug},


    };
}
