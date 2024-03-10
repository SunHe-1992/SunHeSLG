/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageShared
{
    public partial class UI_HintPage : GComponent
    {
        public UI_NumberItem numberComp;
        public Transition gold_fly;
        public const string URL = "ui://9bv6j664mnvhib";

        public static UI_HintPage CreateInstance()
        {
            return (UI_HintPage)UIPackage.CreateObject("PackageShared", "HintPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            numberComp = (UI_NumberItem)GetChild("numberComp");
            gold_fly = GetTransition("gold_fly");
        }
    }
}