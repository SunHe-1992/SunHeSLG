/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_QuestUI : GComponent
    {
        public GImage bg;
        public GButton btn_close;
        public GList list_board;
        public GList list_myQuests;
        public const string URL = "ui://fstosj6i73rdm4";

        public static UI_QuestUI CreateInstance()
        {
            return (UI_QuestUI)UIPackage.CreateObject("PackageBattle", "QuestUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            bg = (GImage)GetChild("bg");
            btn_close = (GButton)GetChild("btn_close");
            list_board = (GList)GetChild("list_board");
            list_myQuests = (GList)GetChild("list_myQuests");
        }
    }
}