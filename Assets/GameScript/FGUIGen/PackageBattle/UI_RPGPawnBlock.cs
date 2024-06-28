/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_RPGPawnBlock : GComponent
    {
        public Controller ctrl_side;
        public GLoader headLoader;
        public GTextField txt_name;
        public const string URL = "ui://fstosj6ivaj1i0";

        public static UI_RPGPawnBlock CreateInstance()
        {
            return (UI_RPGPawnBlock)UIPackage.CreateObject("PackageBattle", "RPGPawnBlock");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_side = GetController("ctrl_side");
            headLoader = (GLoader)GetChild("headLoader");
            txt_name = (GTextField)GetChild("txt_name");
        }
    }
}