/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace PackageDebug
{
    public class PackageDebugBinder
    {
        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_SamplePage.URL, typeof(UI_SamplePage));
            UIObjectFactory.SetPackageItemExtension(UI_TestUI.URL, typeof(UI_TestUI));
        }
    }
}