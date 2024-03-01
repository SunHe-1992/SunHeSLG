/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_0 : GButton
    {
        public Controller mode_;
        public UI_Com_RedPoint red;
        public const string URL = "ui://z2dx9rj5latbojsu";

        public static UI_Button_Common_0 CreateInstance()
        {
            return (UI_Button_Common_0)UIPackage.CreateObject("CommonButton", "Button_Common_0");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            mode_ = GetController("mode_");
            red = (UI_Com_RedPoint)GetChild("red");
        }
    }
}