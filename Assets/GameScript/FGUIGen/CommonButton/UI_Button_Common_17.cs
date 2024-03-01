/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_17 : GButton
    {
        public Controller red;
        public Controller moneyIcon;
        public const string URL = "ui://z2dx9rj5ef5a7sokh7";

        public static UI_Button_Common_17 CreateInstance()
        {
            return (UI_Button_Common_17)UIPackage.CreateObject("CommonButton", "Button_Common_17");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            moneyIcon = GetController("moneyIcon");
        }
    }
}