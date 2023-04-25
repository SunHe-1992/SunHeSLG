/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_AttackHintCom : GComponent
    {
        public Controller ctrl_arrow;
        public Controller ctrl_hint;
        public GTextField txt_value1;
        public GTextField txt_value2;
        public const string URL = "ui://fstosj6ig0n5x";

        public static UI_AttackHintCom CreateInstance()
        {
            return (UI_AttackHintCom)UIPackage.CreateObject("PackageBattle", "AttackHintCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_arrow = GetController("ctrl_arrow");
            ctrl_hint = GetController("ctrl_hint");
            txt_value1 = (GTextField)GetChild("txt_value1");
            txt_value2 = (GTextField)GetChild("txt_value2");
        }
    }
}