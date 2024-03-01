/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_playerClothCom : GComponent
    {
        public Controller choose;
        public Controller iswear;
        public Controller unLock;
        public Controller red;
        public GLoader loader_icon;
        public GRichTextField txt_name;
        public GTextField txt_getName;
        public GTextField txt_getNotice;
        public const string URL = "ui://080sa613rtaxo5z";

        public static UI_playerClothCom CreateInstance()
        {
            return (UI_playerClothCom)UIPackage.CreateObject("CommonPackage", "playerClothCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            choose = GetController("choose");
            iswear = GetController("iswear");
            unLock = GetController("unLock");
            red = GetController("red");
            loader_icon = (GLoader)GetChild("loader_icon");
            txt_name = (GRichTextField)GetChild("txt_name");
            txt_getName = (GTextField)GetChild("txt_getName");
            txt_getNotice = (GTextField)GetChild("txt_getNotice");
        }
    }
}