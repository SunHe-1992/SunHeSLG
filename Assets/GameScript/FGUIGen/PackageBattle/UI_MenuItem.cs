/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_MenuItem : GButton
    {
        public GTextField txt_des;
        public GLoader clickLoader;
        public const string URL = "ui://fstosj6ilay8k";

        public static UI_MenuItem CreateInstance()
        {
            return (UI_MenuItem)UIPackage.CreateObject("PackageBattle", "MenuItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_des = (GTextField)GetChild("txt_des");
            clickLoader = (GLoader)GetChild("clickLoader");
        }
    }
}