/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_9 : GButton
    {
        public Controller type;
        public Controller hasRed;
        public const string URL = "ui://z2dx9rj5etaxsojzw";

        public static UI_Button_Common_9 CreateInstance()
        {
            return (UI_Button_Common_9)UIPackage.CreateObject("CommonButton", "Button_Common_9");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type = GetController("type");
            hasRed = GetController("hasRed");
        }
    }
}