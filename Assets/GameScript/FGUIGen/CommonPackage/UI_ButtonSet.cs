/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_ButtonSet : GButton
    {
        public Controller type;
        public Controller red;
        public const string URL = "ui://080sa613ina81s";

        public static UI_ButtonSet CreateInstance()
        {
            return (UI_ButtonSet)UIPackage.CreateObject("CommonPackage", "ButtonSet");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            type = GetController("type");
            red = GetController("red");
        }
    }
}