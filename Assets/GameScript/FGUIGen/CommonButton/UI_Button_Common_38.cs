/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_38 : GButton
    {
        public Controller page;
        public const string URL = "ui://z2dx9rj5d2s5hhk0tv";

        public static UI_Button_Common_38 CreateInstance()
        {
            return (UI_Button_Common_38)UIPackage.CreateObject("CommonButton", "Button_Common_38");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            page = GetController("page");
        }
    }
}