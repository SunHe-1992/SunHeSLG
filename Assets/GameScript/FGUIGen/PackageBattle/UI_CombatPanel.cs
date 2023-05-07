/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatPanel : GComponent
    {
        public UI_PawnWeaponCom weaponLeft;
        public UI_PawnWeaponCom weaponRight;
        public UI_CombatBar infoLeft;
        public UI_CombatBar infoRight;
        public UI_CombatBar1 hpbar;
        public Transition anim_hide;
        public const string URL = "ui://fstosj6itjjs15";

        public static UI_CombatPanel CreateInstance()
        {
            return (UI_CombatPanel)UIPackage.CreateObject("PackageBattle", "CombatPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            weaponLeft = (UI_PawnWeaponCom)GetChild("weaponLeft");
            weaponRight = (UI_PawnWeaponCom)GetChild("weaponRight");
            infoLeft = (UI_CombatBar)GetChild("infoLeft");
            infoRight = (UI_CombatBar)GetChild("infoRight");
            hpbar = (UI_CombatBar1)GetChild("hpbar");
            anim_hide = GetTransition("anim_hide");
        }
    }
}