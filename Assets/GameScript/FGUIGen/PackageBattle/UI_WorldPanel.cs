/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_WorldPanel : GComponent
    {
        public Controller showMiniGame;
        public Controller showNPCBtn;
        public GButton btn_test;
        public GComponent topHead;
        public GTextField txt_hud;
        public GButton btn_minigame;
        public GButton btn_NPC;
        public GButton btn_inventory;
        public GButton btn_quest;
        public const string URL = "ui://fstosj6ipy0fhw";

        public static UI_WorldPanel CreateInstance()
        {
            return (UI_WorldPanel)UIPackage.CreateObject("PackageBattle", "WorldPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            showMiniGame = GetController("showMiniGame");
            showNPCBtn = GetController("showNPCBtn");
            btn_test = (GButton)GetChild("btn_test");
            topHead = (GComponent)GetChild("topHead");
            txt_hud = (GTextField)GetChild("txt_hud");
            btn_minigame = (GButton)GetChild("btn_minigame");
            btn_NPC = (GButton)GetChild("btn_NPC");
            btn_inventory = (GButton)GetChild("btn_inventory");
            btn_quest = (GButton)GetChild("btn_quest");
        }
    }
}