/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_StatsCom : GComponent
    {
        public Controller ctrl_arrow;
        public GTextField txt_attrName;
        public GTextField txt_attrValue;
        public GTextField txt_changeValue;
        public const string URL = "ui://fstosj6igenvn";

        public static UI_StatsCom CreateInstance()
        {
            return (UI_StatsCom)UIPackage.CreateObject("PackageBattle", "StatsCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_arrow = GetController("ctrl_arrow");
            txt_attrName = (GTextField)GetChild("txt_attrName");
            txt_attrValue = (GTextField)GetChild("txt_attrValue");
            txt_changeValue = (GTextField)GetChild("txt_changeValue");
        }
    }
}