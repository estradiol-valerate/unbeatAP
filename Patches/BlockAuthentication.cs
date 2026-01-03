using System.Threading.Tasks;
using HarmonyLib;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(AuthenticationBase))]
public class BlockAuthentication
{
    // This blocks authentication to the servers. Prevents leaderboard submission
    [HarmonyPatch("CheckConnection")]
    [HarmonyPrefix]
    static bool BlockAuthenticationPatch(ref Task<bool> __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        __result = Task.FromResult(false);
        return false;
    }
}
