/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_SlotGame : GComponent
    {
        public UI_SlotWheel wheel1;
        public UI_SlotWheel wheel2;
        public UI_SlotWheel wheel3;
        public GButton btn_slot;
        public Transition anim_frame;
        public const string URL = "ui://dxvwggiw7irf1d";

        public static UI_SlotGame CreateInstance()
        {
            return (UI_SlotGame)UIPackage.CreateObject("PackageMinigame", "SlotGame");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            wheel1 = (UI_SlotWheel)GetChild("wheel1");
            wheel2 = (UI_SlotWheel)GetChild("wheel2");
            wheel3 = (UI_SlotWheel)GetChild("wheel3");
            btn_slot = (GButton)GetChild("btn_slot");
            anim_frame = GetTransition("anim_frame");
        }
    }
}