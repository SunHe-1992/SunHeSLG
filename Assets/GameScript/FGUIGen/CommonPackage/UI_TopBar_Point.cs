/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_TopBar_Point : GButton
    {
        public Controller hasTime;
        public Controller hasAddBtn;
        public GLoader loader_pic;
        public GRichTextField txt_num;
        public GRichTextField txt_time;
        public const string URL = "ui://080sa613g4vvs";

        public static UI_TopBar_Point CreateInstance()
        {
            return (UI_TopBar_Point)UIPackage.CreateObject("CommonPackage", "TopBar_Point");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            hasTime = GetController("hasTime");
            hasAddBtn = GetController("hasAddBtn");
            loader_pic = (GLoader)GetChild("loader_pic");
            txt_num = (GRichTextField)GetChild("txt_num");
            txt_time = (GRichTextField)GetChild("txt_time");
        }
    }
}