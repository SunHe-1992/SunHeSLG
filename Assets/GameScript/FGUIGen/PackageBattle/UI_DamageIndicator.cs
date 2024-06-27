/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_DamageIndicator : GComponent
    {
        public GLoader pivot;
        public GTextField txt_yellow;
        public GTextField txt_green;
        public Transition t0;
        public const string URL = "ui://fstosj6ilvtsig";

        public static UI_DamageIndicator CreateInstance()
        {
            return (UI_DamageIndicator)UIPackage.CreateObject("PackageBattle", "DamageIndicator");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            pivot = (GLoader)GetChild("pivot");
            txt_yellow = (GTextField)GetChild("txt_yellow");
            txt_green = (GTextField)GetChild("txt_green");
            t0 = GetTransition("t0");
        }
    }
}