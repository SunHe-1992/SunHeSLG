/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_SmallBagPageButton : GButton
    {
        public Controller light;
        public Controller smallButtonType;
        public GLoader gray;
        public GLoader light_2;
        public const string URL = "ui://080sa613f4qrp";

        public static UI_SmallBagPageButton CreateInstance()
        {
            return (UI_SmallBagPageButton)UIPackage.CreateObject("CommonPackage", "SmallBagPageButton");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            light = GetController("light");
            smallButtonType = GetController("smallButtonType");
            gray = (GLoader)GetChild("gray");
            light_2 = (GLoader)GetChild("light");
        }
    }
}