/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_34 : GButton
    {
        public Controller page;
        public Controller btnPage;
        public GImage bg;
        public GLoader noChoose;
        public GLoader choose;
        public const string URL = "ui://z2dx9rj5ptvyhhk0ta";

        public static UI_Button_Common_34 CreateInstance()
        {
            return (UI_Button_Common_34)UIPackage.CreateObject("CommonButton", "Button_Common_34");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            page = GetController("page");
            btnPage = GetController("btnPage");
            bg = (GImage)GetChild("bg");
            noChoose = (GLoader)GetChild("noChoose");
            choose = (GLoader)GetChild("choose");
        }
    }
}