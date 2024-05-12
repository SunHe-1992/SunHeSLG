/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_Fishing : GComponent
    {
        public UI_FishingCircle compCircle;
        public GTextField txt_rodCount;
        public GLoader clickLoader;
        public const string URL = "ui://dxvwggiwpkni1q";

        public static UI_Fishing CreateInstance()
        {
            return (UI_Fishing)UIPackage.CreateObject("PackageMinigame", "Fishing");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            compCircle = (UI_FishingCircle)GetChild("compCircle");
            txt_rodCount = (GTextField)GetChild("txt_rodCount");
            clickLoader = (GLoader)GetChild("clickLoader");
        }
    }
}