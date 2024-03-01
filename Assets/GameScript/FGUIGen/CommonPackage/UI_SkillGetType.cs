/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_SkillGetType : GButton
    {
        public Controller type_;
        public const string URL = "ui://080sa613hkx4o8b";

        public static UI_SkillGetType CreateInstance()
        {
            return (UI_SkillGetType)UIPackage.CreateObject("CommonPackage", "SkillGetType");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type_ = GetController("type_");
        }
    }
}