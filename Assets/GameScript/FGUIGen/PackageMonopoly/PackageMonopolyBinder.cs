/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace PackageMonopoly
{
    public class PackageMonopolyBinder
    {
        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_Construction.URL, typeof(UI_Construction));
            UIObjectFactory.SetPackageItemExtension(UI_BuildingItem.URL, typeof(UI_BuildingItem));
            UIObjectFactory.SetPackageItemExtension(UI_DotItem.URL, typeof(UI_DotItem));
            UIObjectFactory.SetPackageItemExtension(UI_MonopolyMain.URL, typeof(UI_MonopolyMain));
            UIObjectFactory.SetPackageItemExtension(UI_DiceFactor.URL, typeof(UI_DiceFactor));
        }
    }
}