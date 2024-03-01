/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_TipsPage : GComponent
    {
        public UI_Tips_Common_0 tipsCom;
        public const string URL = "ui://080sa613jjga53";

        public static UI_TipsPage CreateInstance()
        {
            return (UI_TipsPage)UIPackage.CreateObject("CommonPackage", "TipsPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            tipsCom = (UI_Tips_Common_0)GetChild("tipsCom");
        }
    }
}