/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_CombatPredict : GComponent
    {
        public UI_CombatPredictCom comLeft;
        public UI_CombatPredictCom comRight;
        public UI_AttackBoutCom comCenter;
        public UI_TileInfo titleInfoLeft;
        public UI_TileInfo titleInfoRight;
        public GButton btn_close;
        public const string URL = "ui://fstosj6ig0n5r";

        public static UI_CombatPredict CreateInstance()
        {
            return (UI_CombatPredict)UIPackage.CreateObject("PackageBattle", "CombatPredict");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            comLeft = (UI_CombatPredictCom)GetChild("comLeft");
            comRight = (UI_CombatPredictCom)GetChild("comRight");
            comCenter = (UI_AttackBoutCom)GetChild("comCenter");
            titleInfoLeft = (UI_TileInfo)GetChild("titleInfoLeft");
            titleInfoRight = (UI_TileInfo)GetChild("titleInfoRight");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}