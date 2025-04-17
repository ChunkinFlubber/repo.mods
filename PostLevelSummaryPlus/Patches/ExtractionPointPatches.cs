using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace PostLevelSummaryPlus.Patches
{
    [HarmonyPatch(typeof(ExtractionPoint))]
    class ExtractionPointPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(ExtractionPoint.ExtractionPointSurplusRPC))]
        public static void StateCompletePostfix(ExtractionPoint __instance)
        {
            if (SemiFunc.RunIsLevel() && PostLevelSummaryPlus.Instance != null)
            {
                PostLevelSummaryPlus.Instance.Level.Extracted(__instance);
            }
        }
    }
}
