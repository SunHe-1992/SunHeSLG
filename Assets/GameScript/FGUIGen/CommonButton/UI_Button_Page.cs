/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Page : GButton
    {
        public Controller typeTouch;
        public GImage bg;
        public const string URL = "ui://z2dx9rj5kbdwsojyp";

        public static UI_Button_Page CreateInstance()
        {
            return (UI_Button_Page)UIPackage.CreateObject("CommonButton", "Button_Page");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            typeTouch = GetController("typeTouch");
            bg = (GImage)GetChild("bg");
        }
    }
}