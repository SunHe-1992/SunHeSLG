/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_DownItem : GComponent
    {
        public Controller type_;
        public Controller choose;
        public Controller red;
        public GLoader loader_icon;
        public const string URL = "ui://080sa613odw31k";

        public static UI_DownItem CreateInstance()
        {
            return (UI_DownItem)UIPackage.CreateObject("CommonPackage", "DownItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type_ = GetController("type_");
            choose = GetController("choose");
            red = GetController("red");
            loader_icon = (GLoader)GetChild("loader_icon");
        }
    }
}