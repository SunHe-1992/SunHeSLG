/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatPredictCom : GComponent
    {
        public Controller ctrl_color;
        public Controller ctrl_showDir;
        public UI_ItemComp weaponCom;
        public GTextField txt_name;
        public GButton btn_prev;
        public GButton btn_next;
        public UI_Label2 lbl_dmg;
        public UI_Label2 lbl_hit;
        public UI_Label2 lbl_crit;
        public UI_HpBar1 hpBar;
        public GTextField txt_hp;
        public const string URL = "ui://fstosj6ig0n5s";

        public static UI_CombatPredictCom CreateInstance()
        {
            return (UI_CombatPredictCom)UIPackage.CreateObject("PackageBattle", "CombatPredictCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_color = GetController("ctrl_color");
            ctrl_showDir = GetController("ctrl_showDir");
            weaponCom = (UI_ItemComp)GetChild("weaponCom");
            txt_name = (GTextField)GetChild("txt_name");
            btn_prev = (GButton)GetChild("btn_prev");
            btn_next = (GButton)GetChild("btn_next");
            lbl_dmg = (UI_Label2)GetChild("lbl_dmg");
            lbl_hit = (UI_Label2)GetChild("lbl_hit");
            lbl_crit = (UI_Label2)GetChild("lbl_crit");
            hpBar = (UI_HpBar1)GetChild("hpBar");
            txt_hp = (GTextField)GetChild("txt_hp");
        }
    }
}