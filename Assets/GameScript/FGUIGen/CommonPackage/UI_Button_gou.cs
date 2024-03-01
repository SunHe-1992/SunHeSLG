/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Button_gou : GButton
    {
        public Controller choose;
        public const string URL = "ui://080sa613hm4wo7o";

        public static UI_Button_gou CreateInstance()
        {
            return (UI_Button_gou)UIPackage.CreateObject("CommonPackage", "Button_gou");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            choose = GetController("choose");
        }
    }
}