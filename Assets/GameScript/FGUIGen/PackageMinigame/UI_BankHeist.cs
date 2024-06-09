/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_BankHeist : GComponent
    {
        public GList list_vault;
        public UI_VaultBar bar1;
        public UI_VaultBar bar2;
        public UI_VaultBar bar3;
        public const string URL = "ui://dxvwggiwxjmq2x";

        public static UI_BankHeist CreateInstance()
        {
            return (UI_BankHeist)UIPackage.CreateObject("PackageMinigame", "BankHeist");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_vault = (GList)GetChild("list_vault");
            bar1 = (UI_VaultBar)GetChild("bar1");
            bar2 = (UI_VaultBar)GetChild("bar2");
            bar3 = (UI_VaultBar)GetChild("bar3");
        }
    }
}