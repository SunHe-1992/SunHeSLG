/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_Change : GButton
    {
        public Transition t0;
        public const string URL = "ui://z2dx9rj5mfmk7sok5s";

        public static UI_Button_Common_Change CreateInstance()
        {
            return (UI_Button_Common_Change)UIPackage.CreateObject("CommonButton", "Button_Common_Change");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            t0 = GetTransition("t0");
        }
    }
}