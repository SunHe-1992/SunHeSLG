/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_SlotWheel : GComponent
    {
        public GList list_icons;
        public const string URL = "ui://dxvwggiw7irf1m";

        public static UI_SlotWheel CreateInstance()
        {
            return (UI_SlotWheel)UIPackage.CreateObject("PackageMinigame", "SlotWheel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_icons = (GList)GetChild("list_icons");
        }
    }
}