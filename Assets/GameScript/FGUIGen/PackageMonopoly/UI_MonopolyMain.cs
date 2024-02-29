/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMonopoly
{
    public partial class UI_MonopolyMain : GComponent
    {
        public GButton btn_Test;
        public GButton btn_Jump;
        public GTextField txt_hud;
        public const string URL = "ui://dxvwggiwr35z0";

        public static UI_MonopolyMain CreateInstance()
        {
            return (UI_MonopolyMain)UIPackage.CreateObject("PackageMonopoly", "MonopolyMain");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_Test = (GButton)GetChild("btn_Test");
            btn_Jump = (GButton)GetChild("btn_Jump");
            txt_hud = (GTextField)GetChild("txt_hud");
        }
    }
}