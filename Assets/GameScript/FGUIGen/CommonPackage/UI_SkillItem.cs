/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_SkillItem : GComponent
    {
        public Controller lockType;
        public UI_skillIcon loader_icon;
        public const string URL = "ui://080sa613qmnzo7r";

        public static UI_SkillItem CreateInstance()
        {
            return (UI_SkillItem)UIPackage.CreateObject("CommonPackage", "SkillItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lockType = GetController("lockType");
            loader_icon = (UI_skillIcon)GetChild("loader_icon");
        }
    }
}