/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_4 : GButton
    {
        public Controller red;
        public Controller hasNum;
        public Controller hasTitle;
        public Controller colorType;
        public UI_IconPointCom loader_icon_1;
        public UI_IconPointCom loader_icon_2;
        public UI_IconPointCom loader_icon_3;
        public GRichTextField txt_notice;
        public const string URL = "ui://z2dx9rj5ijmisojwh";

        public static UI_Button_Common_4 CreateInstance()
        {
            return (UI_Button_Common_4)UIPackage.CreateObject("CommonButton", "Button_Common_4");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            hasNum = GetController("hasNum");
            hasTitle = GetController("hasTitle");
            colorType = GetController("colorType");
            loader_icon_1 = (UI_IconPointCom)GetChild("loader_icon_1");
            loader_icon_2 = (UI_IconPointCom)GetChild("loader_icon_2");
            loader_icon_3 = (UI_IconPointCom)GetChild("loader_icon_3");
            txt_notice = (GRichTextField)GetChild("txt_notice");
        }
    }
}