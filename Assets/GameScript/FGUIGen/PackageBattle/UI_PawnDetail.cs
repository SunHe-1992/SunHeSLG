/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_PawnDetail : GComponent
    {
        public GTextField txt_pawnName;
        public GTextField txt_class;
        public GTextField txt_classType;
        public GTextField txt_move;
        public GList list_combatStats;
        public GList list_BasicStats;
        public UI_StatsCom stat_build;
        public UI_StatsCom stat_SP;
        public UI_HPComp stat_HP;
        public const string URL = "ui://fstosj6igenvm";

        public static UI_PawnDetail CreateInstance()
        {
            return (UI_PawnDetail)UIPackage.CreateObject("PackageBattle", "PawnDetail");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_pawnName = (GTextField)GetChild("txt_pawnName");
            txt_class = (GTextField)GetChild("txt_class");
            txt_classType = (GTextField)GetChild("txt_classType");
            txt_move = (GTextField)GetChild("txt_move");
            list_combatStats = (GList)GetChild("list_combatStats");
            list_BasicStats = (GList)GetChild("list_BasicStats");
            stat_build = (UI_StatsCom)GetChild("stat_build");
            stat_SP = (UI_StatsCom)GetChild("stat_SP");
            stat_HP = (UI_HPComp)GetChild("stat_HP");
        }
    }
}