/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_QuestEntry : GComponent
    {
        public Controller ctrl_status;
        public UI_InventoryItem itemHead;
        public GTextField txt_quest;
        public GButton btn_accept;
        public const string URL = "ui://fstosj6i73rdm5";

        public static UI_QuestEntry CreateInstance()
        {
            return (UI_QuestEntry)UIPackage.CreateObject("PackageBattle", "QuestEntry");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_status = GetController("ctrl_status");
            itemHead = (UI_InventoryItem)GetChild("itemHead");
            txt_quest = (GTextField)GetChild("txt_quest");
            btn_accept = (GButton)GetChild("btn_accept");
        }
    }
}