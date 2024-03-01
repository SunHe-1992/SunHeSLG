/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_itemComWithName : GComponent
    {
        public UI_itemCom item;
        public GRichTextField txt_addNum;
        public GRichTextField txt_name;
        public const string URL = "ui://080sa613rtaxo5m";

        public static UI_itemComWithName CreateInstance()
        {
            return (UI_itemComWithName)UIPackage.CreateObject("CommonPackage", "itemComWithName");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            item = (UI_itemCom)GetChild("item");
            txt_addNum = (GRichTextField)GetChild("txt_addNum");
            txt_name = (GRichTextField)GetChild("txt_name");
        }
    }
}