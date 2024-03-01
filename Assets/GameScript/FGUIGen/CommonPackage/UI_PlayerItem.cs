/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_PlayerItem : GComponent
    {
        public Controller infoType;
        public Controller choose;
        public GRichTextField title;
        public const string URL = "ui://080sa613hm4wo6q";

        public static UI_PlayerItem CreateInstance()
        {
            return (UI_PlayerItem)UIPackage.CreateObject("CommonPackage", "PlayerItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            infoType = GetController("infoType");
            choose = GetController("choose");
            title = (GRichTextField)GetChild("title");
        }
    }
}