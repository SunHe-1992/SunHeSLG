/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_loaderhead : GComponent
    {
        public GLoader loader_head;
        public const string URL = "ui://080sa613g4vv11";

        public static UI_loaderhead CreateInstance()
        {
            return (UI_loaderhead)UIPackage.CreateObject("CommonPackage", "loaderhead");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            loader_head = (GLoader)GetChild("loader_head");
        }
    }
}