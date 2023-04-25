/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_HpBar1 : GProgressBar
    {
        public Controller color;
        public GImage pin;
        public const string URL = "ui://fstosj6ig0n5v";

        public static UI_HpBar1 CreateInstance()
        {
            return (UI_HpBar1)UIPackage.CreateObject("PackageBattle", "HpBar1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            color = GetController("color");
            pin = (GImage)GetChild("pin");
        }
    }
}