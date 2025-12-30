using UnityEngine;
using Rhythm;
namespace UNBEATAP.Traps;

public static class Fading
{
    private static bool enableFade;
    public static void EnableFade(bool enabled)
    {
        enableFade = enabled;
    }
    public static bool GetFade()
    {
        return enableFade;
    }
}