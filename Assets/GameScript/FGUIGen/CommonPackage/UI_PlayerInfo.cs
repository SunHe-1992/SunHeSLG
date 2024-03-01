/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_PlayerInfo : GComponent
    {
        public UI_Com_Herohead head;
        public GButton btn_changeName;
        public GRichTextField txt_name;
        public GRichTextField txt_allNum;
        public GRichTextField txt_winNum;
        public GRichTextField txt_duanwei;
        public GRichTextField txt_allNum2;
        public GRichTextField txt_winNum2;
        public GRichTextField txt_duanwei2;
        public GList list_page;
        public GRichTextField txt_fightNum;
        public GRichTextField txt_fanshuType;
        public GRichTextField txt_winNumOnce;
        public GRichTextField txt_fanshu;
        public GList list_group;
        public GRichTextField txt_UID;
        public const string URL = "ui://080sa613hm4wo79";

        public static UI_PlayerInfo CreateInstance()
        {
            return (UI_PlayerInfo)UIPackage.CreateObject("CommonPackage", "PlayerInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            head = (UI_Com_Herohead)GetChild("head");
            btn_changeName = (GButton)GetChild("btn_changeName");
            txt_name = (GRichTextField)GetChild("txt_name");
            txt_allNum = (GRichTextField)GetChild("txt_allNum");
            txt_winNum = (GRichTextField)GetChild("txt_winNum");
            txt_duanwei = (GRichTextField)GetChild("txt_duanwei");
            txt_allNum2 = (GRichTextField)GetChild("txt_allNum2");
            txt_winNum2 = (GRichTextField)GetChild("txt_winNum2");
            txt_duanwei2 = (GRichTextField)GetChild("txt_duanwei2");
            list_page = (GList)GetChild("list_page");
            txt_fightNum = (GRichTextField)GetChild("txt_fightNum");
            txt_fanshuType = (GRichTextField)GetChild("txt_fanshuType");
            txt_winNumOnce = (GRichTextField)GetChild("txt_winNumOnce");
            txt_fanshu = (GRichTextField)GetChild("txt_fanshu");
            list_group = (GList)GetChild("list_group");
            txt_UID = (GRichTextField)GetChild("txt_UID");
        }
    }
}