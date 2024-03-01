/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_1 : GButton
    {
        public Controller red;
        public Controller type;
        public const string URL = "ui://z2dx9rj5cenmojuq";

        public static UI_Button_Common_1 CreateInstance()
        {
            return (UI_Button_Common_1)UIPackage.CreateObject("CommonButton", "Button_Common_1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            type = GetController("type");
        }
    }
}