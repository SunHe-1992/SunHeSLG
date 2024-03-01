/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_UIMjItem : GComponent
    {
        public Controller ctrl_LT;
        public Controller ctrl_dir;
        public Controller isBack;
        public Controller choose;
        public Controller grayMask;
        public GLoader loader_hor;
        public GLoader loader_hor_back;
        public GLoader loader_ver;
        public GLoader loader_ver_back;
        public GLoader mjLoader;
        public const string URL = "ui://080sa613yvwe3y";

        public static UI_UIMjItem CreateInstance()
        {
            return (UI_UIMjItem)UIPackage.CreateObject("CommonPackage", "UIMjItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_LT = GetController("ctrl_LT");
            ctrl_dir = GetController("ctrl_dir");
            isBack = GetController("isBack");
            choose = GetController("choose");
            grayMask = GetController("grayMask");
            loader_hor = (GLoader)GetChild("loader_hor");
            loader_hor_back = (GLoader)GetChild("loader_hor_back");
            loader_ver = (GLoader)GetChild("loader_ver");
            loader_ver_back = (GLoader)GetChild("loader_ver_back");
            mjLoader = (GLoader)GetChild("mjLoader");
        }
    }
}