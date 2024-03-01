/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_35 : GButton
    {
        public Controller page;
        public GImage bg;
        public const string URL = "ui://z2dx9rj5ptvyhhk0tc";

        public static UI_Button_Common_35 CreateInstance()
        {
            return (UI_Button_Common_35)UIPackage.CreateObject("CommonButton", "Button_Common_35");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            page = GetController("page");
            bg = (GImage)GetChild("bg");
        }
    }
}