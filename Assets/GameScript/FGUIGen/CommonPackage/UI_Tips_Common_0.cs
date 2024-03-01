/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Tips_Common_0 : GLabel
    {
        public Controller hasLayer;
        public GRichTextField txt_layer;
        public const string URL = "ui://080sa613t3vu2v";

        public static UI_Tips_Common_0 CreateInstance()
        {
            return (UI_Tips_Common_0)UIPackage.CreateObject("CommonPackage", "Tips_Common_0");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            hasLayer = GetController("hasLayer");
            txt_layer = (GRichTextField)GetChild("txt_layer");
        }
    }
}