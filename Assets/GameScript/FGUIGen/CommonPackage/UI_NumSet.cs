/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace CommonPackage
{
    public partial class UI_NumSet : GComponent
    {
        public GButton btn_add;
        public GButton btn_del;
        public GTextInput input_num;
        public UI_BtnNumSet_All btn_min;
        public UI_BtnNumSet_All btn_max;
        public const string URL = "ui://080sa613rtaxo5s";

        public static UI_NumSet CreateInstance()
        {
            return (UI_NumSet)UIPackage.CreateObject("CommonPackage", "NumSet");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            btn_add = (GButton)GetChild("btn_add");
            btn_del = (GButton)GetChild("btn_del");
            input_num = (GTextInput)GetChild("input_num");
            btn_min = (UI_BtnNumSet_All)GetChild("btn_min");
            btn_max = (UI_BtnNumSet_All)GetChild("btn_max");
        }
    }
}