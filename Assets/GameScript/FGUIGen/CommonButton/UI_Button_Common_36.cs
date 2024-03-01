/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_36 : GButton
    {
        public GImage img;
        public const string URL = "ui://z2dx9rj5hm4whhk0th";

        public static UI_Button_Common_36 CreateInstance()
        {
            return (UI_Button_Common_36)UIPackage.CreateObject("CommonButton", "Button_Common_36");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            img = (GImage)GetChild("img");
        }
    }
}