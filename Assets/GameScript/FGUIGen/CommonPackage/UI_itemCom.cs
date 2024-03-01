/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_itemCom : GComponent
    {
        public Controller choose;
        public Controller isup;
        public Controller bg;
        public Controller red;
        public Controller useing;
        public Controller nowear;
        public Controller unLock;
        public GLoader bgLoader;
        public GLoader loader_icon;
        public GRichTextField txt_num;
        public GRichTextField txt_tip;
        public Transition ChangeSize;
        public const string URL = "ui://080sa613g4vva";

        public static UI_itemCom CreateInstance()
        {
            return (UI_itemCom)UIPackage.CreateObject("CommonPackage", "itemCom");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            choose = GetController("choose");
            isup = GetController("isup");
            bg = GetController("bg");
            red = GetController("red");
            useing = GetController("useing");
            nowear = GetController("nowear");
            unLock = GetController("unLock");
            bgLoader = (GLoader)GetChild("bgLoader");
            loader_icon = (GLoader)GetChild("loader_icon");
            txt_num = (GRichTextField)GetChild("txt_num");
            txt_tip = (GRichTextField)GetChild("txt_tip");
            ChangeSize = GetTransition("ChangeSize");
        }
    }
}