/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_2 : GButton
    {
        public Controller type;
        public Controller hasIcon;
        public GLoader loader_icon;
        public GTextField txt_cost;
        public const string URL = "ui://z2dx9rj5ku0wojv8";

        public static UI_Button_Common_2 CreateInstance()
        {
            return (UI_Button_Common_2)UIPackage.CreateObject("CommonButton", "Button_Common_2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type = GetController("type");
            hasIcon = GetController("hasIcon");
            loader_icon = (GLoader)GetChild("loader_icon");
            txt_cost = (GTextField)GetChild("txt_cost");
        }
    }
}