/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_15 : GButton
    {
        public Controller red;
        public Controller touchType;
        public GImage bg1;
        public GImage bg2;
        public GLoader buttonTouch;
        public const string URL = "ui://z2dx9rj5avem7sok94";

        public static UI_Button_Common_15 CreateInstance()
        {
            return (UI_Button_Common_15)UIPackage.CreateObject("CommonButton", "Button_Common_15");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            touchType = GetController("touchType");
            bg1 = (GImage)GetChild("bg1");
            bg2 = (GImage)GetChild("bg2");
            buttonTouch = (GLoader)GetChild("buttonTouch");
        }
    }
}