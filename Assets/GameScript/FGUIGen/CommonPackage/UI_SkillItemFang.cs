/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_SkillItemFang : GComponent
    {
        public Controller lockType;
        public GLoader loader_icon;
        public const string URL = "ui://080sa613j9d9o84";

        public static UI_SkillItemFang CreateInstance()
        {
            return (UI_SkillItemFang)UIPackage.CreateObject("CommonPackage", "SkillItemFang");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lockType = GetController("lockType");
            loader_icon = (GLoader)GetChild("loader_icon");
        }
    }
}