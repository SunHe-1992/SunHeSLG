/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatHpBar : GProgressBar
    {
        public Controller color;
        public const string URL = "ui://fstosj6itjjs19";

        public static UI_CombatHpBar CreateInstance()
        {
            return (UI_CombatHpBar)UIPackage.CreateObject("PackageBattle", "CombatHpBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            color = GetController("color");
        }
    }
}