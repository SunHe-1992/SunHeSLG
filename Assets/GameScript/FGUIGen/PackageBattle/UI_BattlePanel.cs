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
        public UI_PhaseComp phaseCom;
        public UI_ActionMenuComp actionCom;
        public UI_ItemInventoryComp inventoryCom;
        public GTextField txt_logicState;
        public Transition anim_phase;
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
            phaseCom = (UI_PhaseComp)GetChild("phaseCom");
            actionCom = (UI_ActionMenuComp)GetChild("actionCom");
            inventoryCom = (UI_ItemInventoryComp)GetChild("inventoryCom");
            txt_logicState = (GTextField)GetChild("txt_logicState");
            anim_phase = GetTransition("anim_phase");
        }
    }
}