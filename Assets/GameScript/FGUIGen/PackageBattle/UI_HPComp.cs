/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_HPComp : GComponent
    {
        public GTextField txt_HP;
        public GTextField txt_HPMAX;
        public UI_ProgressBar1 pgsBar;
        public const string URL = "ui://fstosj6igenvp";

        public static UI_HPComp CreateInstance()
        {
            return (UI_HPComp)UIPackage.CreateObject("PackageBattle", "HPComp");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_HP = (GTextField)GetChild("txt_HP");
            txt_HPMAX = (GTextField)GetChild("txt_HPMAX");
            pgsBar = (UI_ProgressBar1)GetChild("pgsBar");
        }
    }
}