/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_PawnSummaryBar : GComponent
    {
        public Controller ctrl_color;
        public GTextField txt_hp1;
        public GTextField txt_hp2;
        public GTextField txt_class;
        public UI_AttributeUnit AU_atk;
        public UI_AttributeUnit AU_spd;
        public UI_AttributeUnit AU_hit;
        public UI_AttributeUnit AU_avo;
        public UI_AttributeUnit AU_def;
        public UI_AttributeUnit AU_res;
        public UI_AttributeUnit AU_mov;
        public UI_AttributeUnit AU_level;
        public GButton btn_detail;
        public const string URL = "ui://fstosj6iscq5a";

        public static UI_PawnSummaryBar CreateInstance()
        {
            return (UI_PawnSummaryBar)UIPackage.CreateObject("PackageBattle", "PawnSummaryBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_color = GetController("ctrl_color");
            txt_hp1 = (GTextField)GetChild("txt_hp1");
            txt_hp2 = (GTextField)GetChild("txt_hp2");
            txt_class = (GTextField)GetChild("txt_class");
            AU_atk = (UI_AttributeUnit)GetChild("AU_atk");
            AU_spd = (UI_AttributeUnit)GetChild("AU_spd");
            AU_hit = (UI_AttributeUnit)GetChild("AU_hit");
            AU_avo = (UI_AttributeUnit)GetChild("AU_avo");
            AU_def = (UI_AttributeUnit)GetChild("AU_def");
            AU_res = (UI_AttributeUnit)GetChild("AU_res");
            AU_mov = (UI_AttributeUnit)GetChild("AU_mov");
            AU_level = (UI_AttributeUnit)GetChild("AU_level");
            btn_detail = (GButton)GetChild("btn_detail");
        }
    }
}