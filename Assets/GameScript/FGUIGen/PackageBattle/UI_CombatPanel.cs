/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatPanel : GComponent
    {
        public GLoader bg;
        public UI_RPGStatsCom stat0;
        public UI_RPGStatsCom stat1;
        public UI_RPGStatsCom stat2;
        public UI_RPGStatsCom stat3;
        public GList actionList;
        public GTextField txt_turn;
        public UI_ActionMenuComp actionCom;
        public UI_VillianStatsCom villian0;
        public UI_VillianStatsCom villian1;
        public UI_VillianStatsCom villian2;
        public UI_VillianStatsCom villian3;
        public UI_DamagePane ComIndicator;
        public GButton btn_test;
        public GTextField txt_hud;
        public Transition anim_hide;
        public const string URL = "ui://fstosj6itjjs15";

        public static UI_CombatPanel CreateInstance()
        {
            return (UI_CombatPanel)UIPackage.CreateObject("PackageBattle", "CombatPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            bg = (GLoader)GetChild("bg");
            stat0 = (UI_RPGStatsCom)GetChild("stat0");
            stat1 = (UI_RPGStatsCom)GetChild("stat1");
            stat2 = (UI_RPGStatsCom)GetChild("stat2");
            stat3 = (UI_RPGStatsCom)GetChild("stat3");
            actionList = (GList)GetChild("actionList");
            txt_turn = (GTextField)GetChild("txt_turn");
            actionCom = (UI_ActionMenuComp)GetChild("actionCom");
            villian0 = (UI_VillianStatsCom)GetChild("villian0");
            villian1 = (UI_VillianStatsCom)GetChild("villian1");
            villian2 = (UI_VillianStatsCom)GetChild("villian2");
            villian3 = (UI_VillianStatsCom)GetChild("villian3");
            ComIndicator = (UI_DamagePane)GetChild("ComIndicator");
            btn_test = (GButton)GetChild("btn_test");
            txt_hud = (GTextField)GetChild("txt_hud");
            anim_hide = GetTransition("anim_hide");
        }
    }
}