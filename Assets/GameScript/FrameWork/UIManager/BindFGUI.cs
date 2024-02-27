
public static class BindFGUI
{

    /// <summary>
    /// 所有界面的binder
    /// </summary>
    public static void BindAll()
    {
        PackageDebug.PackageDebugBinder.BindAll();
        PackageBattle.PackageBattleBinder.BindAll();
        PackageShared.PackageSharedBinder.BindAll();
        PackageMonopoly.PackageMonopolyBinder.BindAll();
    }
}
