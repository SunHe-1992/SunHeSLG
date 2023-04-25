/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageShared
{
    public partial class UI_ButtonArrow : GButton
    {
        public Controller dir;
        public const string URL = "ui://9bv6j664g0n5i8";

        public static UI_ButtonArrow CreateInstance()
        {
            return (UI_ButtonArrow)UIPackage.CreateObject("PackageShared", "ButtonArrow");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            dir = GetController("dir");
        }
    }
}