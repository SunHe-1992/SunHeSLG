/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_UIMJRow : GComponent
    {
        public Controller choose;
        public GList list_mj;
        public const string URL = "ui://080sa613jjga52";

        public static UI_UIMJRow CreateInstance()
        {
            return (UI_UIMJRow)UIPackage.CreateObject("CommonPackage", "UIMJRow");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            choose = GetController("choose");
            list_mj = (GList)GetChild("list_mj");
        }
    }
}