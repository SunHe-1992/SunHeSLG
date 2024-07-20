/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_SettingUI : GComponent
    {
        public GButton btn_close;
        public GTextField txt_title;
        public GSlider volume_slider;
        public const string URL = "ui://fstosj6iv94blz";

        public static UI_SettingUI CreateInstance()
        {
            return (UI_SettingUI)UIPackage.CreateObject("PackageBattle", "SettingUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_close = (GButton)GetChild("btn_close");
            txt_title = (GTextField)GetChild("txt_title");
            volume_slider = (GSlider)GetChild("volume_slider");
        }
    }
}