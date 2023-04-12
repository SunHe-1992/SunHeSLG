/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_PhaseComp : GComponent
    {
        public Controller ctrl_phase;
        public const string URL = "ui://fstosj6iag5lj";

        public static UI_PhaseComp CreateInstance()
        {
            return (UI_PhaseComp)UIPackage.CreateObject("PackageBattle", "PhaseComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_phase = GetController("ctrl_phase");
        }
    }
}