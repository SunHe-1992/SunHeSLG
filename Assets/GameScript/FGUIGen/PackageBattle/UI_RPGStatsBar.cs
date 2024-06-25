/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_RPGStatsBar : GComponent
    {
        public GTextField txt_title;
        public UI_CombatHpBar comBar;
        public GTextField txt_value;
        public GTextField txt_valueMax;
        public const string URL = "ui://fstosj6ivaj1hy";

        public static UI_RPGStatsBar CreateInstance()
        {
            return (UI_RPGStatsBar)UIPackage.CreateObject("PackageBattle", "RPGStatsBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_title = (GTextField)GetChild("txt_title");
            comBar = (UI_CombatHpBar)GetChild("comBar");
            txt_value = (GTextField)GetChild("txt_value");
            txt_valueMax = (GTextField)GetChild("txt_valueMax");
        }
    }
}