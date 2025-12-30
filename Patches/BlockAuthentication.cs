using System.Threading.Tasks;
using HarmonyLib;
namespace UNBEATAP.Patches
{
    public class BlockAuthentication
    {
        // This blocks authentication to the servers. Prevents leaderboard submission
        [HarmonyPatch(typeof(AuthenticationBase), "CheckConnection")]
        [HarmonyPrefix]
        static bool BlockAuthenticationPatch(ref Task<bool> __result)
        {
            __result = Task.FromResult(false);
            return false;
        }
    }
}