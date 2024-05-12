/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_FishingCircle : GComponent
    {
        public Controller ctrl_status;
        public GGraph targetCircle;
        public GGraph motionCircle;
        public GMovieClip effect_gold;
        public GMovieClip effect_blue;
        public Transition anim_waiting;
        public Transition anim_good;
        public Transition anim_excellent;
        public Transition anim_bad;
        public const string URL = "ui://dxvwggiwpkni1s";

        public static UI_FishingCircle CreateInstance()
        {
            return (UI_FishingCircle)UIPackage.CreateObject("PackageMinigame", "FishingCircle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_status = GetController("ctrl_status");
            targetCircle = (GGraph)GetChild("targetCircle");
            motionCircle = (GGraph)GetChild("motionCircle");
            effect_gold = (GMovieClip)GetChild("effect_gold");
            effect_blue = (GMovieClip)GetChild("effect_blue");
            anim_waiting = GetTransition("anim_waiting");
            anim_good = GetTransition("anim_good");
            anim_excellent = GetTransition("anim_excellent");
            anim_bad = GetTransition("anim_bad");
        }
    }
}