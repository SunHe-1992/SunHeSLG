/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_WeaponStats : GComponent
    {
        public Controller ctrl_state;
        public GList list_attrs;
        public GTextField txt_des;
        public const string URL = "ui://fstosj6itjjs11";

        public static UI_WeaponStats CreateInstance()
        {
            return (UI_WeaponStats)UIPackage.CreateObject("PackageBattle", "WeaponStats");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_state = GetController("ctrl_state");
            list_attrs = (GList)GetChild("list_attrs");
            txt_des = (GTextField)GetChild("txt_des");
        }
    }
}