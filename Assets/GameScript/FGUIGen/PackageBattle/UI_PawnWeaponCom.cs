/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_PawnWeaponCom : GComponent
    {
        public Controller color;
        public GTextField txt_name;
        public UI_BagItemComp weaponCom;
        public const string URL = "ui://fstosj6itjjs16";

        public static UI_PawnWeaponCom CreateInstance()
        {
            return (UI_PawnWeaponCom)UIPackage.CreateObject("PackageBattle", "PawnWeaponCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            color = GetController("color");
            txt_name = (GTextField)GetChild("txt_name");
            weaponCom = (UI_BagItemComp)GetChild("weaponCom");
        }
    }
}