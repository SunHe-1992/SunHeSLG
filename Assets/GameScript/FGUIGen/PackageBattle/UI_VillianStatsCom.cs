/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_VillianStatsCom : GComponent
    {
        public Controller ctrl_dead;
        public Controller ctrl_selected;
        public UI_RPGStatsBar stats;
        public const string URL = "ui://fstosj6ivaj1i2";

        public static UI_VillianStatsCom CreateInstance()
        {
            return (UI_VillianStatsCom)UIPackage.CreateObject("PackageBattle", "VillianStatsCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_dead = GetController("ctrl_dead");
            ctrl_selected = GetController("ctrl_selected");
            stats = (UI_RPGStatsBar)GetChild("stats");
        }
    }
}