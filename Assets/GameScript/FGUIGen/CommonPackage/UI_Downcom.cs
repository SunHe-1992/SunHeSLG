/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_Downcom : GComponent
    {
        public UI_DownItem btn_paiyou;
        public UI_DownItem btn_bag;
        public UI_DownItem btn_paipu;
        public UI_DownItem btn_shop;
        public UI_DownItem btn_chalou;
        public const string URL = "ui://080sa613odw31d";

        public static UI_Downcom CreateInstance()
        {
            return (UI_Downcom)UIPackage.CreateObject("CommonPackage", "Downcom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_paiyou = (UI_DownItem)GetChild("btn_paiyou");
            btn_bag = (UI_DownItem)GetChild("btn_bag");
            btn_paipu = (UI_DownItem)GetChild("btn_paipu");
            btn_shop = (UI_DownItem)GetChild("btn_shop");
            btn_chalou = (UI_DownItem)GetChild("btn_chalou");
        }
    }
}