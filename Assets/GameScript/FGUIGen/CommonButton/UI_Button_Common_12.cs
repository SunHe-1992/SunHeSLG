/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_12 : GButton
    {
        public Controller red;
        public GImage bg1;
        public GImage bg2;
        public GLoader buttonTouch;
        public const string URL = "ui://z2dx9rj5l2mn7sok1r";

        public static UI_Button_Common_12 CreateInstance()
        {
            return (UI_Button_Common_12)UIPackage.CreateObject("CommonButton", "Button_Common_12");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            bg1 = (GImage)GetChild("bg1");
            bg2 = (GImage)GetChild("bg2");
            buttonTouch = (GLoader)GetChild("buttonTouch");
        }
    }
}