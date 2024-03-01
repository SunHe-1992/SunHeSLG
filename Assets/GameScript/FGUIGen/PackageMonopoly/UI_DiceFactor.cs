/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMonopoly
{
    public partial class UI_DiceFactor : GButton
    {
        public Controller ctrl_color;
        public const string URL = "ui://dxvwggiww6ez4";

        public static UI_DiceFactor CreateInstance()
        {
            return (UI_DiceFactor)UIPackage.CreateObject("PackageMonopoly", "DiceFactor");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_color = GetController("ctrl_color");
        }
    }
}