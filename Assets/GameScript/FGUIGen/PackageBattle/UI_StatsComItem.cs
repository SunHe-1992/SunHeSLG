/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_StatsComItem : GComponent
    {
        public Controller ctrl_arrow;
        public GTextField txt_attrName;
        public GTextField txt_attrValue;
        public const string URL = "ui://fstosj6itjjs12";

        public static UI_StatsComItem CreateInstance()
        {
            return (UI_StatsComItem)UIPackage.CreateObject("PackageBattle", "StatsComItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_arrow = GetController("ctrl_arrow");
            txt_attrName = (GTextField)GetChild("txt_attrName");
            txt_attrValue = (GTextField)GetChild("txt_attrValue");
        }
    }
}