/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_32 : GButton
    {
        public Controller type_;
        public Controller red;
        public GImage bg;
        public const string URL = "ui://z2dx9rj5rtaxhhk0t5";

        public static UI_Button_Common_32 CreateInstance()
        {
            return (UI_Button_Common_32)UIPackage.CreateObject("CommonButton", "Button_Common_32");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type_ = GetController("type_");
            red = GetController("red");
            bg = (GImage)GetChild("bg");
        }
    }
}