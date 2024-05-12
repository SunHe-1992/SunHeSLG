/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_SlotIconCom : GComponent
    {
        public Controller ctrl_icon;
        public const string URL = "ui://dxvwggiw7irf1l";

        public static UI_SlotIconCom CreateInstance()
        {
            return (UI_SlotIconCom)UIPackage.CreateObject("PackageMinigame", "SlotIconCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_icon = GetController("ctrl_icon");
        }
    }
}