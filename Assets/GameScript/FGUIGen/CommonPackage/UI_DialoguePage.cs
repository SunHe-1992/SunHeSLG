/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_DialoguePage : GComponent
    {
        public GImage bg;
        public GLoader portraitLoader;
        public GTextField txt_name;
        public GTextField txt_content;
        public GList list_options;
        public GLoader bgLoader;
        public const string URL = "ui://080sa613ensiiy";

        public static UI_DialoguePage CreateInstance()
        {
            return (UI_DialoguePage)UIPackage.CreateObject("CommonPackage", "DialoguePage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            bg = (GImage)GetChild("bg");
            portraitLoader = (GLoader)GetChild("portraitLoader");
            txt_name = (GTextField)GetChild("txt_name");
            txt_content = (GTextField)GetChild("txt_content");
            list_options = (GList)GetChild("list_options");
            bgLoader = (GLoader)GetChild("bgLoader");
        }
    }
}