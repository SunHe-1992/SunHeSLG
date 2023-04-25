/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_AttackBoutCom : GComponent
    {
        public Controller showEngage;
        public GList list_atk;
        public const string URL = "ui://fstosj6ig0n5w";

        public static UI_AttackBoutCom CreateInstance()
        {
            return (UI_AttackBoutCom)UIPackage.CreateObject("PackageBattle", "AttackBoutCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            showEngage = GetController("showEngage");
            list_atk = (GList)GetChild("list_atk");
        }
    }
}