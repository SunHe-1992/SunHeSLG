/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Button_playerChoose : GButton
    {
        public Controller choose;
        public const string URL = "ui://080sa613hm4wo78";

        public static UI_Button_playerChoose CreateInstance()
        {
            return (UI_Button_playerChoose)UIPackage.CreateObject("CommonPackage", "Button_playerChoose");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            choose = GetController("choose");
        }
    }
}