/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_rideYuanFen : GButton
    {
        public Controller red;
        public UI_Com_RedPoint red_2;
        public const string URL = "ui://z2dx9rj5qpk9hhk0r7";

        public static UI_Button_rideYuanFen CreateInstance()
        {
            return (UI_Button_rideYuanFen)UIPackage.CreateObject("CommonButton", "Button_rideYuanFen");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            red_2 = (UI_Com_RedPoint)GetChild("red");
        }
    }
}