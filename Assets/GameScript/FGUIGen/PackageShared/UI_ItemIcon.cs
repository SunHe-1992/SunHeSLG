/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageShared
{
    public partial class UI_ItemIcon : GComponent
    {
        public GLoader iconLoader;
        public const string URL = "ui://9bv6j664g0n5hv";

        public static UI_ItemIcon CreateInstance()
        {
            return (UI_ItemIcon)UIPackage.CreateObject("PackageShared", "ItemIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            iconLoader = (GLoader)GetChild("iconLoader");
        }
    }
}