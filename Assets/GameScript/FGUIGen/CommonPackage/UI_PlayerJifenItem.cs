/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_PlayerJifenItem : GComponent
    {
        public Controller winType;
        public Controller shoucang;
        public GRichTextField txt_name;
        public GRichTextField txt_time;
        public GRichTextField txt_jifen;
        public GLoader loader_icon;
        public GButton btn_shoucang;
        public const string URL = "ui://080sa613hm4wo7k";

        public static UI_PlayerJifenItem CreateInstance()
        {
            return (UI_PlayerJifenItem)UIPackage.CreateObject("CommonPackage", "PlayerJifenItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            winType = GetController("winType");
            shoucang = GetController("shoucang");
            txt_name = (GRichTextField)GetChild("txt_name");
            txt_time = (GRichTextField)GetChild("txt_time");
            txt_jifen = (GRichTextField)GetChild("txt_jifen");
            loader_icon = (GLoader)GetChild("loader_icon");
            btn_shoucang = (GButton)GetChild("btn_shoucang");
        }
    }
}