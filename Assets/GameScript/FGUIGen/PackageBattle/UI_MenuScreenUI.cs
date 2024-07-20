/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_MenuScreenUI : GComponent
    {
        public Controller ctrl_mode;
        public GButton btn_newGame;
        public GButton btn_RPGGame;
        public GButton btn_loadGame;
        public GButton btn_options;
        public GButton btn_exit;
        public GTextField txt_title;
        public const string URL = "ui://fstosj6iv94blx";

        public static UI_MenuScreenUI CreateInstance()
        {
            return (UI_MenuScreenUI)UIPackage.CreateObject("PackageBattle", "MenuScreenUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_mode = GetController("ctrl_mode");
            btn_newGame = (GButton)GetChild("btn_newGame");
            btn_RPGGame = (GButton)GetChild("btn_RPGGame");
            btn_loadGame = (GButton)GetChild("btn_loadGame");
            btn_options = (GButton)GetChild("btn_options");
            btn_exit = (GButton)GetChild("btn_exit");
            txt_title = (GTextField)GetChild("txt_title");
        }
    }
}