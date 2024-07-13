/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_InventoryItem : GComponent
    {
        public Controller ctrl_quality;
        public Controller button;
        public GLoader iconLoader;
        public GTextField txt_count;
        public GTextField txt_name;
        public const string URL = "ui://fstosj6igm4tj7";

        public static UI_InventoryItem CreateInstance()
        {
            return (UI_InventoryItem)UIPackage.CreateObject("PackageBattle", "InventoryItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_quality = GetController("ctrl_quality");
            button = GetController("button");
            iconLoader = (GLoader)GetChild("iconLoader");
            txt_count = (GTextField)GetChild("txt_count");
            txt_name = (GTextField)GetChild("txt_name");
        }
    }
}