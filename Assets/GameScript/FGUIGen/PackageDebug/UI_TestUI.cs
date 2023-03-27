/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageDebug
{
    public partial class UI_TestUI : GComponent
    {
        public GButton btn_test;
        public const string URL = "ui://arg2zso7pk0k0";

        public static UI_TestUI CreateInstance()
        {
            return (UI_TestUI)UIPackage.CreateObject("PackageDebug", "TestUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_test = (GButton)GetChild("btn_test");
        }
    }
}