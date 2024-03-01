/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_commonKuang : GComponent
    {
        public GImage kuang;
        public GButton btn_close;
        public const string URL = "ui://080sa613rtaxo5j";

        public static UI_commonKuang CreateInstance()
        {
            return (UI_commonKuang)UIPackage.CreateObject("CommonPackage", "commonKuang");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            kuang = (GImage)GetChild("kuang");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}