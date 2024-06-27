/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageBattle
{
    public partial class UI_DamagePane : GComponent
    {
        public GList list_pool;
        public const string URL = "ui://fstosj6ilvtsiv";

        public static UI_DamagePane CreateInstance()
        {
            return (UI_DamagePane)UIPackage.CreateObject("PackageBattle", "DamagePane");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_pool = (GList)GetChild("list_pool");
        }
    }
}