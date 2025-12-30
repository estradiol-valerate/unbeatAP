using Arcade.Progression;
using HarmonyLib;
namespace UNBEATAP.Patches
{
    public class UnlockAll
    {
        // This is particularly useful because it doesn't change progression on your save slot at all! It simply bypasses the unlock check entirely
        [HarmonyPatch(typeof(MainProgressionContainer), "UnlockAll", MethodType.Getter)]
        [HarmonyPostfix]
        static void unlockAll(ref bool __result)
        {
            __result = true;
        }
    }
}