/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_18 : GButton
    {
        public Controller red;
        public GLoader moneyLoader;
        public const string URL = "ui://z2dx9rj5ef5a7sokha";

        public static UI_Button_Common_18 CreateInstance()
        {
            return (UI_Button_Common_18)UIPackage.CreateObject("CommonButton", "Button_Common_18");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            moneyLoader = (GLoader)GetChild("moneyLoader");
        }
    }
}