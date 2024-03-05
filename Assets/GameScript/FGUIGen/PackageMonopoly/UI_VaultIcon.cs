/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMonopoly
{
    public partial class UI_VaultIcon : GComponent
    {
        public Controller ctrl_active;
        public Controller ctrl_icon;
        public GLoader iconLoader;
        public const string URL = "ui://dxvwggiwlhbz18";

        public static UI_VaultIcon CreateInstance()
        {
            return (UI_VaultIcon)UIPackage.CreateObject("PackageMonopoly", "VaultIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_active = GetController("ctrl_active");
            ctrl_icon = GetController("ctrl_icon");
            iconLoader = (GLoader)GetChild("iconLoader");
        }
    }
}