/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_ItemComp : GComponent
    {
        public GTextField txt_name;
        public UI_ItemIcon iconCom;
        public const string URL = "ui://fstosj6ig0n5hu";

        public static UI_ItemComp CreateInstance()
        {
            return (UI_ItemComp)UIPackage.CreateObject("PackageBattle", "ItemComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_name = (GTextField)GetChild("txt_name");
            iconCom = (UI_ItemIcon)GetChild("iconCom");
        }
    }
}