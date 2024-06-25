/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_RPGStatsCom : GComponent
    {
        public Controller ctrl_highlight;
        public GTextField txt_name;
        public UI_RPGStatsBar HPCom;
        public UI_RPGStatsBar SPCom;
        public const string URL = "ui://fstosj6ivaj1hz";

        public static UI_RPGStatsCom CreateInstance()
        {
            return (UI_RPGStatsCom)UIPackage.CreateObject("PackageBattle", "RPGStatsCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_highlight = GetController("ctrl_highlight");
            txt_name = (GTextField)GetChild("txt_name");
            HPCom = (UI_RPGStatsBar)GetChild("HPCom");
            SPCom = (UI_RPGStatsBar)GetChild("SPCom");
        }
    }
}