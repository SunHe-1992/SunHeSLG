/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button3 : GButton
    {
        public Controller red;
        public const string URL = "ui://z2dx9rj5ef5a7sokh8";

        public static UI_Button3 CreateInstance()
        {
            return (UI_Button3)UIPackage.CreateObject("CommonButton", "Button3");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
        }
    }
}