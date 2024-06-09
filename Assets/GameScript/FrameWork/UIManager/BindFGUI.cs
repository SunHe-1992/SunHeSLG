
public static class BindFGUI
{

    /// <summary>
    /// 所有界面的binder
    /// </summary>
    public static void BindAll()
    {
        CommonButton.CommonButtonBinder.BindAll();
        CommonPackage.CommonPackageBinder.BindAll();
        PackageDebug.PackageDebugBinder.BindAll();
        PackageBattle.PackageBattleBinder.BindAll();
        PackageShared.PackageSharedBinder.BindAll();
        PackageMinigame.PackageMinigameBinder.BindAll();
    }
}
