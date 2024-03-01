/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_13 : GButton
    {
        public Controller type;
        public Controller red;
        public Transition t0;
        public const string URL = "ui://z2dx9rj5lwwu7sok1x";

        public static UI_Button_Common_13 CreateInstance()
        {
            return (UI_Button_Common_13)UIPackage.CreateObject("CommonButton", "Button_Common_13");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type = GetController("type");
            red = GetController("red");
            t0 = GetTransition("t0");
        }
    }
}