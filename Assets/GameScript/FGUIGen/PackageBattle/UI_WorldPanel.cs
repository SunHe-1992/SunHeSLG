/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_WorldPanel : GComponent
    {
        public GButton btn_test;
        public GComponent topHead;
        public GTextField txt_hud;
        public const string URL = "ui://fstosj6ipy0fhw";

        public static UI_WorldPanel CreateInstance()
        {
            return (UI_WorldPanel)UIPackage.CreateObject("PackageBattle", "WorldPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_test = (GButton)GetChild("btn_test");
            topHead = (GComponent)GetChild("topHead");
            txt_hud = (GTextField)GetChild("txt_hud");
        }
    }
}