/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_PlayerInfoSet : GComponent
    {
        public Controller playerInfoType;
        public GButton btn_close;
        public GList list_page;
        public UI_PlayerInfo PlayerInfo;
        public GRichTextField txt_shengju;
        public GRichTextField txt_shengjusaiji;
        public GRichTextField txt_shengjupaiwei;
        public GList list_shegnju;
        public GList list_saishi;
        public GGroup group_rongyu;
        public GList list_zhanjiType;
        public GList list_zhanjiInfo;
        public GRichTextField txt_infoshow;
        public UI_Button_gou btn_allowSee;
        public const string URL = "ui://080sa613di7j1b";

        public static UI_PlayerInfoSet CreateInstance()
        {
            return (UI_PlayerInfoSet)UIPackage.CreateObject("CommonPackage", "PlayerInfoSet");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            playerInfoType = GetController("playerInfoType");
            btn_close = (GButton)GetChild("btn_close");
            list_page = (GList)GetChild("list_page");
            PlayerInfo = (UI_PlayerInfo)GetChild("PlayerInfo");
            txt_shengju = (GRichTextField)GetChild("txt_shengju");
            txt_shengjusaiji = (GRichTextField)GetChild("txt_shengjusaiji");
            txt_shengjupaiwei = (GRichTextField)GetChild("txt_shengjupaiwei");
            list_shegnju = (GList)GetChild("list_shegnju");
            list_saishi = (GList)GetChild("list_saishi");
            group_rongyu = (GGroup)GetChild("group_rongyu");
            list_zhanjiType = (GList)GetChild("list_zhanjiType");
            list_zhanjiInfo = (GList)GetChild("list_zhanjiInfo");
            txt_infoshow = (GRichTextField)GetChild("txt_infoshow");
            btn_allowSee = (UI_Button_gou)GetChild("btn_allowSee");
        }
    }
}