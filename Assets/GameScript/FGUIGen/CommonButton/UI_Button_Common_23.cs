/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_23 : GButton
    {
        public Controller type;
        public const string URL = "ui://z2dx9rj5ckcthhk0sx";

        public static UI_Button_Common_23 CreateInstance()
        {
            return (UI_Button_Common_23)UIPackage.CreateObject("CommonButton", "Button_Common_23");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type = GetController("type");
        }
    }
}