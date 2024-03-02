/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMonopoly
{
    public partial class UI_BuildingItem : GButton
    {
        public GLoader loader_pic;
        public GTextField txt_price;
        public UI_DotItem dotItem;
        public GTextField txt_name;
        public const string URL = "ui://dxvwggiwei10m";

        public static UI_BuildingItem CreateInstance()
        {
            return (UI_BuildingItem)UIPackage.CreateObject("PackageMonopoly", "BuildingItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            loader_pic = (GLoader)GetChild("loader_pic");
            txt_price = (GTextField)GetChild("txt_price");
            dotItem = (UI_DotItem)GetChild("dotItem");
            txt_name = (GTextField)GetChild("txt_name");
        }
    }
}