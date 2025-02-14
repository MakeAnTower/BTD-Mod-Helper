﻿using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
namespace BTD_Mod_Helper.Patches.InputManagers;

[HarmonyPatch(typeof(InputManager), nameof(InputManager.PrimeTower))]
internal class InputManager_PrimeTower
{
    [HarmonyPostfix]
    internal static void Postfix(InputManager __instance)
    {
        if (InGame.instance == null)
            return;

        TaskScheduler.ScheduleTask(() => { ModHelper.PerformHook(mod => mod.OnTowerGraphicsCreated(__instance.placementModel, __instance.placementGraphics)); }, waitCondition: () => __instance.placementGraphics?.Count > 0);
    }
}