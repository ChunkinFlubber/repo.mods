using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace PostLevelSummary.Patches
{
    [HarmonyPatch(typeof(ExtractionPoint))]
    class ExtractionPointPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(ExtractionPoint.ExtractionPointSurplusRPC))]
        public static void StateCompletePostfix(ExtractionPoint __instance)
        {
            if (PostLevelSummary.InGame)
            {
                PostLevelSummary.Level.Extracted(__instance);
            }
        }
    }
}
