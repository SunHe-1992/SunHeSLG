/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Common_RuleText : GComponent
    {
        public GRichTextField title;
        public const string URL = "ui://080sa613di7jo6k";

        public static UI_Common_RuleText CreateInstance()
        {
            return (UI_Common_RuleText)UIPackage.CreateObject("CommonPackage", "Common_RuleText");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            title = (GRichTextField)GetChild("title");
        }
    }
}