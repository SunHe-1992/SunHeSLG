/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_skillIcon : GComponent
    {
        public Controller lockType;
        public GLoader loader_icon;
        public const string URL = "ui://080sa613vim2o8a";

        public static UI_skillIcon CreateInstance()
        {
            return (UI_skillIcon)UIPackage.CreateObject("CommonPackage", "skillIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lockType = GetController("lockType");
            loader_icon = (GLoader)GetChild("loader_icon");
        }
    }
}