/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_IconPointCom : GComponent
    {
        public GLoader loader_icon;
        public GRichTextField txt_num;
        public const string URL = "ui://z2dx9rj5guux7sok1p";

        public static UI_IconPointCom CreateInstance()
        {
            return (UI_IconPointCom)UIPackage.CreateObject("CommonButton", "IconPointCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            loader_icon = (GLoader)GetChild("loader_icon");
            txt_num = (GRichTextField)GetChild("txt_num");
        }
    }
}