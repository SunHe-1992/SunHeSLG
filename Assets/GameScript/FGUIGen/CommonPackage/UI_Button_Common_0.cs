/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Button_Common_0 : GButton
    {
        public Controller red;
        public Controller buttonColor;
        public const string URL = "ui://080sa613ina81p";

        public static UI_Button_Common_0 CreateInstance()
        {
            return (UI_Button_Common_0)UIPackage.CreateObject("CommonPackage", "Button_Common_0");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            buttonColor = GetController("buttonColor");
        }
    }
}