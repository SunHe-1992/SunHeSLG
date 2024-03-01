/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_ConfirmWindow : GComponent
    {
        public Controller ctrl_showBtn;
        public GGraph bg;
        public GRichTextField txt_content;
        public GButton btnYes;
        public GButton btnNo;
        public GButton btnYes2;
        public const string URL = "ui://080sa613ibfjo7q";

        public static UI_ConfirmWindow CreateInstance()
        {
            return (UI_ConfirmWindow)UIPackage.CreateObject("CommonPackage", "ConfirmWindow");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_showBtn = GetController("ctrl_showBtn");
            bg = (GGraph)GetChild("bg");
            txt_content = (GRichTextField)GetChild("txt_content");
            btnYes = (GButton)GetChild("btnYes");
            btnNo = (GButton)GetChild("btnNo");
            btnYes2 = (GButton)GetChild("btnYes2");
        }
    }
}