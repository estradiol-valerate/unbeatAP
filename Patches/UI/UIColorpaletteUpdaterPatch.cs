using Arcade.UI;
using HarmonyLib;
using UBUI.Colors;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(UIColorPaletteUpdater))]
public class UIColorPaletteUpdaterPatch
{
    [HarmonyPatch("UpdatePalette")]
    [HarmonyPostfix]
    static void UpdatePalettePostfix()
    {
        string selectedPalette = UIColorPaletteUpdater.SelectedPalette;
        if(string.IsNullOrEmpty(selectedPalette))
        {
            return;
        }

        if(!MenuPaletteIndex.CachedDefaultIndex.TryGetPalette(selectedPalette, out MenuPaletteIndex.Palette palette))
        {
            return;
        }

        if(palette == null)
        {
            return;
        }

        if(palette.palette.colors.Length == 0)
        {
            return;
        }

        if(!ColorManager.Instance)
        {
            return;
        }

        ColorManager.Instance.SetColors(palette.palette.colors);
    }
}