/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_UnitUI : GComponent
    {
        public GGraph bg;
        public UI_PawnDetail pawn_detail;
        public GButton btn_close;
        public const string URL = "ui://fstosj6igenvq";

        public static UI_UnitUI CreateInstance()
        {
            return (UI_UnitUI)UIPackage.CreateObject("PackageBattle", "UnitUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            bg = (GGraph)GetChild("bg");
            pawn_detail = (UI_PawnDetail)GetChild("pawn_detail");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}