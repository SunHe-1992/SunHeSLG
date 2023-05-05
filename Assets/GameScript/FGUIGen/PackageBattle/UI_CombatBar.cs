/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatBar : GComponent
    {
        public GTextField txt_value1;
        public GTextField txt_value2;
        public GTextField txt_value3;
        public const string URL = "ui://fstosj6itjjs17";

        public static UI_CombatBar CreateInstance()
        {
            return (UI_CombatBar)UIPackage.CreateObject("PackageBattle", "CombatBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_value1 = (GTextField)GetChild("txt_value1");
            txt_value2 = (GTextField)GetChild("txt_value2");
            txt_value3 = (GTextField)GetChild("txt_value3");
        }
    }
}