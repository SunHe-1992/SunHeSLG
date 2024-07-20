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
        BattlePanel,
        BattlePrepare,
        UnitUI,
        CombatPredict,
        CombatPanel,
        HintPage,
        SlotGame,
        Fishing,
        BankHeist,
        WorldPanel,
        CombatEndUI,
        DialoguePage,
        InventoryUI,
        StoreUI,
        MenuScreenUI,
        SettingUI,
    }
    /// <summary>
    /// package names
    /// </summary>
    public enum FPackage
    {
        PackageDebug,
        PackageShared,
        PackageBattle,
        PackageMinigame,
        CommonPackage,
        CommonButton,
    }
    /// <summary>
    /// dic : key=window name, value=package name
    /// </summary>
    public static Dictionary<FWindow, FPackage> windowUIpair = new Dictionary<FWindow, FPackage>()
    {
        {FWindow.TestUI, FPackage.PackageDebug},
        {FWindow.SamplePage, FPackage.PackageDebug},
        {FWindow.BattlePanel, FPackage.PackageBattle},
        {FWindow.BattlePrepare, FPackage.PackageBattle},
        {FWindow.UnitUI, FPackage.PackageBattle},
        {FWindow.CombatPredict, FPackage.PackageBattle},
        {FWindow.CombatPanel, FPackage.PackageBattle},
        {FWindow.HintPage, FPackage.PackageShared},
        {FWindow.SlotGame, FPackage.PackageMinigame},
        {FWindow.Fishing, FPackage.PackageMinigame},
        {FWindow.BankHeist, FPackage.PackageMinigame},
        {FWindow.WorldPanel, FPackage.PackageBattle},
        {FWindow.CombatEndUI, FPackage.PackageBattle},
        {FWindow.DialoguePage, FPackage.CommonPackage},
        {FWindow.InventoryUI, FPackage.PackageBattle},
        {FWindow.StoreUI, FPackage.PackageBattle},
        {FWindow.MenuScreenUI, FPackage.PackageBattle},
        {FWindow.SettingUI, FPackage.PackageBattle},
    };
}
