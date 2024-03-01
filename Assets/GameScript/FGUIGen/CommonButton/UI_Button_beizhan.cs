/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_beizhan : GButton
    {
        public Controller red;
        public GImage btn_beizhan;
        public const string URL = "ui://z2dx9rj5p6k07sok8o";

        public static UI_Button_beizhan CreateInstance()
        {
            return (UI_Button_beizhan)UIPackage.CreateObject("CommonButton", "Button_beizhan");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            btn_beizhan = (GImage)GetChild("btn_beizhan");
        }
    }
}