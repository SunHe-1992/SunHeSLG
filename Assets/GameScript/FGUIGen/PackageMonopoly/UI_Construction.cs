/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMonopoly
{
    public partial class UI_Construction : GComponent
    {
        public GComponent topBar;
        public GList list_building;
        public GButton btn_close;
        public GTextField txt_count;
        public GTextField txt_story;
        public const string URL = "ui://dxvwggiwei10l";

        public static UI_Construction CreateInstance()
        {
            return (UI_Construction)UIPackage.CreateObject("PackageMonopoly", "Construction");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            topBar = (GComponent)GetChild("topBar");
            list_building = (GList)GetChild("list_building");
            btn_close = (GButton)GetChild("btn_close");
            txt_count = (GTextField)GetChild("txt_count");
            txt_story = (GTextField)GetChild("txt_story");
        }
    }
}