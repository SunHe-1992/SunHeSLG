/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageShared
{
    public partial class UI_ItemComp : GComponent
    {
        public GTextField txt_name;
        public UI_ItemIcon iconCom;
        public const string URL = "ui://9bv6j664g0n5hu";

        public static UI_ItemComp CreateInstance()
        {
            return (UI_ItemComp)UIPackage.CreateObject("PackageShared", "ItemComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_name = (GTextField)GetChild("txt_name");
            iconCom = (UI_ItemIcon)GetChild("iconCom");
        }
    }
}