/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_StoreUI : GComponent
    {
        public GButton btn_close;
        public GList inventory_list;
        public UI_ItemDetail itemDetailCom;
        public const string URL = "ui://fstosj6i7ot0lw";

        public static UI_StoreUI CreateInstance()
        {
            return (UI_StoreUI)UIPackage.CreateObject("PackageBattle", "StoreUI");
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