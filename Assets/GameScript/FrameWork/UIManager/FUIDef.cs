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
        MonopolyMain,
        Construction,
        HintPage,
        BankHeist,
        SlotGame
    }
    /// <summary>
    /// package names
    /// </summary>
    public enum FPackage
    {
        PackageDebug,
        PackageShared,
        PackageBattle,
        PackageMonopoly,
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
        {FWindow.MonopolyMain, FPackage.PackageMonopoly},
        {FWindow.Construction, FPackage.PackageMonopoly},
        {FWindow.HintPage, FPackage.PackageShared},
        {FWindow.BankHeist, FPackage.PackageMonopoly},
        {FWindow.SlotGame, FPackage.PackageMonopoly},
    };
}
