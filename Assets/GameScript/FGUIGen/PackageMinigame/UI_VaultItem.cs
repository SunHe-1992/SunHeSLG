/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_VaultItem : GComponent
    {
        public Controller ctrl_opened;
        public Controller ctrl_icon;
        public GLoader iconLoader;
        public const string URL = "ui://dxvwggiwxjmq30";

        public static UI_VaultItem CreateInstance()
        {
            return (UI_VaultItem)UIPackage.CreateObject("PackageMinigame", "VaultItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_opened = GetController("ctrl_opened");
            ctrl_icon = GetController("ctrl_icon");
            iconLoader = (GLoader)GetChild("iconLoader");
        }
    }
}