/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_19 : GButton
    {
        public Controller red;
        public Controller moneyIcon;
        public Controller type;
        public GTextField title_money;
        public GLoader loader_money_icon;
        public const string URL = "ui://z2dx9rj5lnzc7sokhb";

        public static UI_Button_Common_19 CreateInstance()
        {
            return (UI_Button_Common_19)UIPackage.CreateObject("CommonButton", "Button_Common_19");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            moneyIcon = GetController("moneyIcon");
            type = GetController("type");
            title_money = (GTextField)GetChild("title_money");
            loader_money_icon = (GLoader)GetChild("loader_money_icon");
        }
    }
}