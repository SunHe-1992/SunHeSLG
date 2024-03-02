/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMonopoly
{
    public partial class UI_DotItem : GComponent
    {
        public Controller ctrl_tier;
        public const string URL = "ui://dxvwggiwei10n";

        public static UI_DotItem CreateInstance()
        {
            return (UI_DotItem)UIPackage.CreateObject("PackageMonopoly", "DotItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_tier = GetController("ctrl_tier");
        }
    }
}