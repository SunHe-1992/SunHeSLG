/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_ButtonSuitIcon : GButton
    {
        public Transition t0;
        public const string URL = "ui://z2dx9rj5y5n47sok0l";

        public static UI_ButtonSuitIcon CreateInstance()
        {
            return (UI_ButtonSuitIcon)UIPackage.CreateObject("CommonButton", "ButtonSuitIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            t0 = GetTransition("t0");
        }
    }
}