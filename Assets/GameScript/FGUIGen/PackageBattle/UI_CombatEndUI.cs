/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatEndUI : GComponent
    {
        public GTextField txt_result;
        public GLoader clickLoader;
        public const string URL = "ui://fstosj6ikrpdix";

        public static UI_CombatEndUI CreateInstance()
        {
            return (UI_CombatEndUI)UIPackage.CreateObject("PackageBattle", "CombatEndUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_result = (GTextField)GetChild("txt_result");
            clickLoader = (GLoader)GetChild("clickLoader");
        }
    }
}