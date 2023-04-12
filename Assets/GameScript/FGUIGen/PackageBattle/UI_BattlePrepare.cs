/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_BattlePrepare : GComponent
    {
        public GButton btn_fight;
        public const string URL = "ui://fstosj6iag5li";

        public static UI_BattlePrepare CreateInstance()
        {
            return (UI_BattlePrepare)UIPackage.CreateObject("PackageBattle", "BattlePrepare");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_fight = (GButton)GetChild("btn_fight");
        }
    }
}