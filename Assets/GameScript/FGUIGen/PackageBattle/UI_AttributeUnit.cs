/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_AttributeUnit : GComponent
    {
        public Controller ctrl_buffed;
        public Controller ctrl_bar;
        public GTextField txt_attrName;
        public GTextField txt_attrValue;
        public UI_ProgressBar1 pgsBar;
        public const string URL = "ui://fstosj6iscq5b";

        public static UI_AttributeUnit CreateInstance()
        {
            return (UI_AttributeUnit)UIPackage.CreateObject("PackageBattle", "AttributeUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_buffed = GetController("ctrl_buffed");
            ctrl_bar = GetController("ctrl_bar");
            txt_attrName = (GTextField)GetChild("txt_attrName");
            txt_attrValue = (GTextField)GetChild("txt_attrValue");
            pgsBar = (UI_ProgressBar1)GetChild("pgsBar");
        }
    }
}