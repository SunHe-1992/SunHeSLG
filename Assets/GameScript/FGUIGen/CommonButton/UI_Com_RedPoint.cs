/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Com_RedPoint : GComponent
    {
        public Controller redpoint;
        public Controller c1;
        public Transition t0;
        public Transition t1;
        public const string URL = "ui://z2dx9rj5latbojsw";

        public static UI_Com_RedPoint CreateInstance()
        {
            return (UI_Com_RedPoint)UIPackage.CreateObject("CommonButton", "Com_RedPoint");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            redpoint = GetController("redpoint");
            c1 = GetController("c1");
            t0 = GetTransition("t0");
            t1 = GetTransition("t1");
        }
    }
}