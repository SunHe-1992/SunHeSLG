/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Com_Herohead : GComponent
    {
        public Controller isTop;
        public UI_loaderhead loadercom;
        public GLoader loaderKuang;
        public const string URL = "ui://080sa613g4vvv";

        public static UI_Com_Herohead CreateInstance()
        {
            return (UI_Com_Herohead)UIPackage.CreateObject("CommonPackage", "Com_Herohead");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            isTop = GetController("isTop");
            loadercom = (UI_loaderhead)GetChild("loadercom");
            loaderKuang = (GLoader)GetChild("loaderKuang");
        }
    }
}