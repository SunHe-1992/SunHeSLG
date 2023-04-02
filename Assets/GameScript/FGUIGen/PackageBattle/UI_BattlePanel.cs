/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_BattlePanel : GComponent
    {
        public UI_PawnNameComp nameBar;
        public UI_PawnSummaryBar bottomBar;
        public UI_TileInfo tileInfoComp;
        public const string URL = "ui://fstosj6iscq58";

        public static UI_BattlePanel CreateInstance()
        {
            return (UI_BattlePanel)UIPackage.CreateObject("PackageBattle", "BattlePanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            nameBar = (UI_PawnNameComp)GetChild("nameBar");
            bottomBar = (UI_PawnSummaryBar)GetChild("bottomBar");
            tileInfoComp = (UI_TileInfo)GetChild("tileInfoComp");
        }
    }
}