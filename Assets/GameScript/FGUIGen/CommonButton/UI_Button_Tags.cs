/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Tags : GComponent
    {
        public Controller lightType;
        public Controller colorType;
        public GLoader img_green;
        public const string URL = "ui://z2dx9rj5inae7sok8s";

        public static UI_Button_Tags CreateInstance()
        {
            return (UI_Button_Tags)UIPackage.CreateObject("CommonButton", "Button_Tags");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lightType = GetController("lightType");
            colorType = GetController("colorType");
            img_green = (GLoader)GetChild("img_green");
        }
    }
}