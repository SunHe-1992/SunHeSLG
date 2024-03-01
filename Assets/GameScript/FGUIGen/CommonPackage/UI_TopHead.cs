/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_TopHead : GComponent
    {
        public Controller topType;
        public UI_Com_Herohead headCom;
        public GRichTextField txt_lvName;
        public GRichTextField txt_UID;
        public GRichTextField txt_name;
        public const string URL = "ui://080sa613g4vvz";

        public static UI_TopHead CreateInstance()
        {
            return (UI_TopHead)UIPackage.CreateObject("CommonPackage", "TopHead");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            topType = GetController("topType");
            headCom = (UI_Com_Herohead)GetChild("headCom");
            txt_lvName = (GRichTextField)GetChild("txt_lvName");
            txt_UID = (GRichTextField)GetChild("txt_UID");
            txt_name = (GRichTextField)GetChild("txt_name");
        }
    }
}