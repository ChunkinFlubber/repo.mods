using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace PostLevelSummary.Patches
{
    [HarmonyPatch(typeof(ValuableObject))]
    class ValuableObjectPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(ValuableObject.DollarValueSetLogic))]
        static void DollarValueSetLogicPostfix(ValuableObject __instance)
        {
            if (__instance.name.ToLower().Contains("surplus"))
                return;

            PostLevelSummary.Level.TotalItems += 1;
        }
    }
}
