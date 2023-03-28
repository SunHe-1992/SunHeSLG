/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageDebug
{
    public partial class UI_TestUI : GComponent
    {
        public GGraph bg;
        public GImage frame;
        public GButton btn_test;
        public GButton btn_slg;
        public const string URL = "ui://arg2zso7pk0k0";

        public static UI_TestUI CreateInstance()
        {
            return (UI_TestUI)UIPackage.CreateObject("PackageDebug", "TestUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            bg = (GGraph)GetChild("bg");
            frame = (GImage)GetChild("frame");
            btn_test = (GButton)GetChild("btn_test");
            btn_slg = (GButton)GetChild("btn_slg");
        }
    }
}