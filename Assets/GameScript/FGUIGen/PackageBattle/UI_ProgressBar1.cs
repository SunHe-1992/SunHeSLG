/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_ProgressBar1 : GProgressBar
    {
        public Controller color;
        public const string URL = "ui://fstosj6iscq5e";

        public static UI_ProgressBar1 CreateInstance()
        {
            return (UI_ProgressBar1)UIPackage.CreateObject("PackageBattle", "ProgressBar1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            color = GetController("color");
        }
    }
}