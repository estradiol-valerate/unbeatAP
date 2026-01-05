using HarmonyLib;
using Rhythm;
using UNBEATAP.Traps;
using UnityEngine;

namespace UNBEATAP.Patches;

public class RainbowTrapPatch
{
    [HarmonyPatch(typeof(StorableBeatmapOptions), "normalNoteColor", MethodType.Getter)]
    [HarmonyPrefix]
    static bool NormalColorPatch(ref Color __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        if(!Rainbow.GetRainbow())
        {
            return true;
        }

        __result = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
        return false;
    }


    [HarmonyPatch(typeof(StorableBeatmapOptions), "multiHitNotesColor", MethodType.Getter)]
    [HarmonyPrefix]
    static bool MultiHitNotesColorPatch(ref Color __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        if(!Rainbow.GetRainbow())
        {
            return true;
        }

        __result = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
        return false;
    }


    [HarmonyPatch(typeof(StorableBeatmapOptions), "laneSwapNoteColor", MethodType.Getter)]
    [HarmonyPrefix]
    static bool LaneSwapNoteColorPatch(ref Color __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        if(!Rainbow.GetRainbow())
        {
            return true;
        }

        __result = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
        return false;
    }


    [HarmonyPatch(typeof(StorableBeatmapOptions), "dodgeNoteColor", MethodType.Getter)]
    [HarmonyPrefix]
    static bool DodgeNoteColorPatch(ref Color __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        if(!Rainbow.GetRainbow())
        {
            return true;
        }

        __result = new Color(Random.Range(0F,1F), Random.Range(0, 1F), Random.Range(0, 1F));
        return false;
    }


    [HarmonyPatch(typeof(DefaultNote), "RhythmUpdate_Moving")]
    [HarmonyPostfix]
    static void DefaultNoteUpdatePostfix(DefaultNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        if(__instance.hidden)
        {
            return;
        }

        // Force override the sprite color so that the notes don't turn pink
        // when they enter the hit range
        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(DoubleNote), "RhythmUpdate_Moving")]
    [HarmonyPostfix]
    static void DoubleNoteUpdatePostfix(DoubleNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        if(__instance.hidden)
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(DoubleNote), "RhythmUpdate_Stunned")]
    [HarmonyPostfix]
    static void DoubleNoteUpdateStunnedPostfix(DoubleNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(FreestyleNote), "RhythmUpdate_Moving")]
    [HarmonyPostfix]
    static void FreestyleNoteUpdatePostfix(FreestyleNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        if(__instance.hidden)
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(FreestyleNote), "RhythmUpdate_Stunned")]
    [HarmonyPostfix]
    static void FreestyleNoteUpdateStunnedPostfix(FreestyleNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        // Also override child note colors
        Color defaultColor = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
        __instance.sprite.color = defaultColor;
        if(__instance.subNote.sprite)
        {
            __instance.subNote.sprite.color = defaultColor;
        }
    }


    [HarmonyPatch(typeof(HoldNote), "RhythmUpdate_Moving")]
    [HarmonyPostfix]
    static void HoldNoteUpdatePostfix(HoldNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        if(__instance.hidden)
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(NothingNote), "Start")]
    [HarmonyPostfix]
    static void NothingNoteStartPostfix(NothingNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        __instance.sprite.color = FileStorage.beatmapOptions.normalNoteColor;
        new Traverse(__instance).Field("defaultColor").SetValue(__instance.sprite.color);
    }


    [HarmonyPatch(typeof(NothingNote), "RhythmUpdate_Moving")]
    [HarmonyPostfix]
    static void NothingNoteUpdatePostfix(NothingNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        if(__instance.hidden)
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(SpamNote), "RhythmUpdate_Moving")]
    [HarmonyPostfix]
    static void SpamNoteUpdatePostfix(SpamNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        if(__instance.hidden)
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }


    [HarmonyPatch(typeof(SpamNote), "RhythmUpdate_Stunned")]
    [HarmonyPostfix]
    static void SpamNoteUpdateStunnedPostfix(SpamNote __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Rainbow.GetRainbow())
        {
            return;
        }

        __instance.sprite.color = new Traverse(__instance).Field("defaultColor").GetValue<Color>();
    }
}