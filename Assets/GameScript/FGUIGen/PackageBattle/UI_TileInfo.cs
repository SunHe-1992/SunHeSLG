/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_TileInfo : GComponent
    {
        public Controller ctrl_showEffects;
        public GTextField txt_tileName;
        public GLabel lbl_passible;
        public GLabel lbl_effect;
        public GLabel lbl_skl;
        public GList list_sklIcons;
        public const string URL = "ui://fstosj6iscq5h";

        public static UI_TileInfo CreateInstance()
        {
            return (UI_TileInfo)UIPackage.CreateObject("PackageBattle", "TileInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_showEffects = GetController("ctrl_showEffects");
            txt_tileName = (GTextField)GetChild("txt_tileName");
            lbl_passible = (GLabel)GetChild("lbl_passible");
            lbl_effect = (GLabel)GetChild("lbl_effect");
            lbl_skl = (GLabel)GetChild("lbl_skl");
            list_sklIcons = (GList)GetChild("list_sklIcons");
        }
    }
}