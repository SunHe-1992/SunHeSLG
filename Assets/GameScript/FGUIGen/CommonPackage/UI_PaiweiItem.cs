/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_PaiweiItem : GComponent
    {
        public GLoader loader_icon;
        public GRichTextField txt_num;
        public const string URL = "ui://080sa613hm4wo7e";

        public static UI_PaiweiItem CreateInstance()
        {
            return (UI_PaiweiItem)UIPackage.CreateObject("CommonPackage", "PaiweiItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            loader_icon = (GLoader)GetChild("loader_icon");
            txt_num = (GRichTextField)GetChild("txt_num");
        }
    }
}