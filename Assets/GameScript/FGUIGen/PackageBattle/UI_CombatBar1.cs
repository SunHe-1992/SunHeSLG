/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatBar1 : GComponent
    {
        public GTextField HPLeft;
        public GTextField HPRight;
        public UI_CombatHpBar barLeft;
        public UI_CombatHpBar barRight;
        public const string URL = "ui://fstosj6itjjs18";

        public static UI_CombatBar1 CreateInstance()
        {
            return (UI_CombatBar1)UIPackage.CreateObject("PackageBattle", "CombatBar1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            HPLeft = (GTextField)GetChild("HPLeft");
            HPRight = (GTextField)GetChild("HPRight");
            barLeft = (UI_CombatHpBar)GetChild("barLeft");
            barRight = (UI_CombatHpBar)GetChild("barRight");
        }
    }
}