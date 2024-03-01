/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonButton
{
    public partial class UI_Button_Common_22 : GButton
    {
        public Controller red;
        public Controller moneyIcon;
        public Controller type;
        public Controller grayType;
        public GTextField title_money;
        public GLoader loader_money_icon;
        public const string URL = "ui://z2dx9rj5j4k6hhk0sn";

        public static UI_Button_Common_22 CreateInstance()
        {
            return (UI_Button_Common_22)UIPackage.CreateObject("CommonButton", "Button_Common_22");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            red = GetController("red");
            moneyIcon = GetController("moneyIcon");
            type = GetController("type");
            grayType = GetController("grayType");
            title_money = (GTextField)GetChild("title_money");
            loader_money_icon = (GLoader)GetChild("loader_money_icon");
        }
    }
}