/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_SkillType : GButton
    {
        public Controller type_;
        public const string URL = "ui://080sa613qmnzo7u";

        public static UI_SkillType CreateInstance()
        {
            return (UI_SkillType)UIPackage.CreateObject("CommonPackage", "SkillType");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type_ = GetController("type_");
        }
    }
}