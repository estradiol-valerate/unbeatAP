using Arcade.Progression;
using HarmonyLib;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(MainProgressionContainer))]
public class UnlockAll
{
    // This is particularly useful because it doesn't change progression on your save slot at all! It simply bypasses the unlock check entirely
    [HarmonyPatch("UnlockAll", MethodType.Getter)]
    [HarmonyPostfix]
    static void UnlockAllPrefix(ref bool __result)
    {
        if(Plugin.Client.Connected)
        {
            __result = true;
        }
    }
}
