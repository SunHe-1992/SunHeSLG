/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_ItemInventoryComp : GComponent
    {
        public GLabel lbl_title;
        public UI_WeaponStats statsComp;
        public GList list_inventory;
        public const string URL = "ui://fstosj6itjjsz";

        public static UI_ItemInventoryComp CreateInstance()
        {
            return (UI_ItemInventoryComp)UIPackage.CreateObject("PackageBattle", "ItemInventoryComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lbl_title = (GLabel)GetChild("lbl_title");
            statsComp = (UI_WeaponStats)GetChild("statsComp");
            list_inventory = (GList)GetChild("list_inventory");
        }
    }
}