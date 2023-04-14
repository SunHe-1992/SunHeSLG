/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_ActionMenuComp : GComponent
    {
        public GList list_action;
        public const string URL = "ui://fstosj6ilay8l";

        public static UI_ActionMenuComp CreateInstance()
        {
            return (UI_ActionMenuComp)UIPackage.CreateObject("PackageBattle", "ActionMenuComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_action = (GList)GetChild("list_action");
        }
    }
}