/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_7 : GButton
    {
        public Controller type;
        public Controller red;
        public Controller iconVisible;
        public UI_Com_RedPoint red_2;
        public GLoader loader_need;
        public const string URL = "ui://z2dx9rj5o5kasojwt";

        public static UI_Button_Common_7 CreateInstance()
        {
            return (UI_Button_Common_7)UIPackage.CreateObject("CommonButton", "Button_Common_7");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type = GetController("type");
            red = GetController("red");
            iconVisible = GetController("iconVisible");
            red_2 = (UI_Com_RedPoint)GetChild("red");
            loader_need = (GLoader)GetChild("loader_need");
        }
    }
}