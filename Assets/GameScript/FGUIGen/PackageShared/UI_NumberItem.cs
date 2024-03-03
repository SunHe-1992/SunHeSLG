/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageShared
{
    public partial class UI_NumberItem : GLabel
    {
        public Transition anim1;
        public const string URL = "ui://9bv6j664mnvhip";

        public static UI_NumberItem CreateInstance()
        {
            return (UI_NumberItem)UIPackage.CreateObject("PackageShared", "NumberItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            anim1 = GetTransition("anim1");
        }
    }
}