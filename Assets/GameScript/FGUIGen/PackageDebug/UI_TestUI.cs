/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageDebug
{
    public partial class UI_TestUI : GComponent
    {
        public GImage frame;
        public GButton btn_test;
        public GButton btn_slg;
        public GButton btn_close;
        public const string URL = "ui://arg2zso7pk0k0";

        public static UI_TestUI CreateInstance()
        {
            return (UI_TestUI)UIPackage.CreateObject("PackageDebug", "TestUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            frame = (GImage)GetChild("frame");
            btn_test = (GButton)GetChild("btn_test");
            btn_slg = (GButton)GetChild("btn_slg");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}