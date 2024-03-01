/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_EmoCom : GComponent
    {
        public Controller choose;
        public Controller red;
        public GLoader loader_icon;
        public const string URL = "ui://080sa613rtaxo5f";

        public static UI_EmoCom CreateInstance()
        {
            return (UI_EmoCom)UIPackage.CreateObject("CommonPackage", "EmoCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            choose = GetController("choose");
            red = GetController("red");
            loader_icon = (GLoader)GetChild("loader_icon");
        }
    }
}