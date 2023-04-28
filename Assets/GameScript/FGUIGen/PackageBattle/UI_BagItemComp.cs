/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_BagItemComp : GButton
    {
        public Controller ctrl_grayed;
        public GTextField txt_name;
        public UI_ItemIcon iconCom;
        public GTextField txt_left;
        public GLoader clickLoader;
        public const string URL = "ui://fstosj6itjjs14";

        public static UI_BagItemComp CreateInstance()
        {
            return (UI_BagItemComp)UIPackage.CreateObject("PackageBattle", "BagItemComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_grayed = GetController("ctrl_grayed");
            txt_name = (GTextField)GetChild("txt_name");
            iconCom = (UI_ItemIcon)GetChild("iconCom");
            txt_left = (GTextField)GetChild("txt_left");
            clickLoader = (GLoader)GetChild("clickLoader");
        }
    }
}