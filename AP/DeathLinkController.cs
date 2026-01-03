using System.Collections.Generic;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using UNBEATAP.Helpers;

namespace UNBEATAP.AP;

public static class DeathLinkController
{
    public static readonly Dictionary<DeathLinkReason, string> DeathLinkMessage = new Dictionary<DeathLinkReason, string>
    {
        {DeathLinkReason.Fail, "{player} couldn't keep up."},
        {DeathLinkReason.Quit, "{player} lacked confidence."}
    };


    public static void TryPerformDeathLink(DeathLink deathLink)
    {
        DeathHelper.KillPlayer();
    }


    public static void SendDeathLink(DeathLinkReason reason)
    {
        if(!Plugin.Client.Connected || !Plugin.Client.deathLink || Plugin.Client.DeathLinkService == null)
        {
            return;
        }

        DeathLinkService service = Plugin.Client.DeathLinkService;
        string message = DeathLinkMessage[reason].Replace("{player}", Plugin.Client.slot);
        DeathLink deathLink = new DeathLink(Plugin.Client.slot, message);

        Plugin.Logger.LogInfo($"Sending death link with message: {message}");
        service.SendDeathLink(deathLink);
    }
}


public enum DeathLinkReason
{
    Fail,
    Quit
}