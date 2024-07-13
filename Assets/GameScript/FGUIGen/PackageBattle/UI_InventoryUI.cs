/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_InventoryUI : GComponent
    {
        public GButton btn_close;
        public GList inventory_list;
        public UI_ItemDetail itemDetailCom;
        public const string URL = "ui://fstosj6igm4tj6";

        public static UI_InventoryUI CreateInstance()
        {
            return (UI_InventoryUI)UIPackage.CreateObject("PackageBattle", "InventoryUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_close = (GButton)GetChild("btn_close");
            inventory_list = (GList)GetChild("inventory_list");
            itemDetailCom = (UI_ItemDetail)GetChild("itemDetailCom");
        }
    }
}