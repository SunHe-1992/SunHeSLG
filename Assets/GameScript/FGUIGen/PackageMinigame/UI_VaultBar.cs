/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace PackageMinigame
{
    public partial class UI_VaultBar : GComponent
    {
        public Controller ctrl_size;
        public UI_VaultIcon item1;
        public UI_VaultIcon item2;
        public UI_VaultIcon item3;
        public GTextField txt_des;
        public Transition anim_shake;
        public const string URL = "ui://dxvwggiwxjmq2y";

        public static UI_VaultBar CreateInstance()
        {
            return (UI_VaultBar)UIPackage.CreateObject("PackageMinigame", "VaultBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_size = GetController("ctrl_size");
            item1 = (UI_VaultIcon)GetChild("item1");
            item2 = (UI_VaultIcon)GetChild("item2");
            item3 = (UI_VaultIcon)GetChild("item3");
            txt_des = (GTextField)GetChild("txt_des");
            anim_shake = GetTransition("anim_shake");
        }
    }
}