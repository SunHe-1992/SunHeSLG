/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_Label2 : GLabel
    {
        public GTextField txt_value;
        public const string URL = "ui://fstosj6ig0n5u";

        public static UI_Label2 CreateInstance()
        {
            return (UI_Label2)UIPackage.CreateObject("PackageBattle", "Label2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_value = (GTextField)GetChild("txt_value");
        }
    }
}