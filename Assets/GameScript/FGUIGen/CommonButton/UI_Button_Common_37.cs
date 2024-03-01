/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_37 : GButton
    {
        public Controller page;
        public GImage bg;
        public const string URL = "ui://z2dx9rj5pf6ahhk0tu";

        public static UI_Button_Common_37 CreateInstance()
        {
            return (UI_Button_Common_37)UIPackage.CreateObject("CommonButton", "Button_Common_37");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            page = GetController("page");
            bg = (GImage)GetChild("bg");
        }
    }
}