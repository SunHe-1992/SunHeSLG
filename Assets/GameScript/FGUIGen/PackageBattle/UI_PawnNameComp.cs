/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_PawnNameComp : GComponent
    {
        public GTextField txt_name;
        public GTextField txt_bond;
        public const string URL = "ui://fstosj6iscq5g";

        public static UI_PawnNameComp CreateInstance()
        {
            return (UI_PawnNameComp)UIPackage.CreateObject("PackageBattle", "PawnNameComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_name = (GTextField)GetChild("txt_name");
            txt_bond = (GTextField)GetChild("txt_bond");
        }
    }
}