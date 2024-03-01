/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Equip : GButton
    {
        public Controller red;
        public const string URL = "ui://z2dx9rj5g9xtojv7";

        public static UI_Button_Equip CreateInstance()
        {
            return (UI_Button_Equip)UIPackage.CreateObject("CommonButton", "Button_Equip");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
        }
    }
}