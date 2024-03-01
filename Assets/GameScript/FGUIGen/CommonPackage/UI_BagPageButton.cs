/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_BagPageButton : GButton
    {
        public Controller touchType;
        public const string URL = "ui://080sa613f4qro";

        public static UI_BagPageButton CreateInstance()
        {
            return (UI_BagPageButton)UIPackage.CreateObject("CommonPackage", "BagPageButton");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            touchType = GetController("touchType");
        }
    }
}