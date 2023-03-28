/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageDebug
{
    public partial class UI_SamplePage : GComponent
    {
        public GGraph bg;
        public GImage frame;
        public GButton btn_ok;
        public const string URL = "ui://arg2zso77rh74";

        public static UI_SamplePage CreateInstance()
        {
            return (UI_SamplePage)UIPackage.CreateObject("PackageDebug", "SamplePage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            bg = (GGraph)GetChild("bg");
            frame = (GImage)GetChild("frame");
            btn_ok = (GButton)GetChild("btn_ok");
        }
    }
}