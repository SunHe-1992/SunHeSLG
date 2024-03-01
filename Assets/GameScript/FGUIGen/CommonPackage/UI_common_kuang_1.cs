/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_common_kuang_1 : GComponent
    {
        public Controller hasClose;
        public GButton btn_close;
        public GRichTextField title;
        public const string URL = "ui://080sa613tjzyo65";

        public static UI_common_kuang_1 CreateInstance()
        {
            return (UI_common_kuang_1)UIPackage.CreateObject("CommonPackage", "common_kuang_1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            hasClose = GetController("hasClose");
            btn_close = (GButton)GetChild("btn_close");
            title = (GRichTextField)GetChild("title");
        }
    }
}