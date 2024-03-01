/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_TopbarCom : GComponent
    {
        public Controller showHead;
        public UI_TopHead head;
        public GRichTextField txt_title;
        public GButton btn_back;
        public UI_TopBar_Point goldComp;
        public const string URL = "ui://080sa613g4vvr";

        public static UI_TopbarCom CreateInstance()
        {
            return (UI_TopbarCom)UIPackage.CreateObject("CommonPackage", "TopbarCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            showHead = GetController("showHead");
            head = (UI_TopHead)GetChild("head");
            txt_title = (GRichTextField)GetChild("txt_title");
            btn_back = (GButton)GetChild("btn_back");
            goldComp = (UI_TopBar_Point)GetChild("goldComp");
        }
    }
}