/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_BtnNumSet_All : GButton
    {
        public Controller upOrDown;
        public const string URL = "ui://080sa613rtaxo5v";

        public static UI_BtnNumSet_All CreateInstance()
        {
            return (UI_BtnNumSet_All)UIPackage.CreateObject("CommonPackage", "BtnNumSet_All");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            upOrDown = GetController("upOrDown");
        }
    }
}