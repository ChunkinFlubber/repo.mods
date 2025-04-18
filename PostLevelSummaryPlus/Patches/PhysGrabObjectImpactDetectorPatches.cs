using HarmonyLib;
using UnityEngine;

namespace PostLevelSummaryPlus.Patches
{
    [HarmonyPatch(typeof(PhysGrabObjectImpactDetector))]
    class PhysGrabObjectImpactDetectorPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(PhysGrabObjectImpactDetector.BreakRPC))]
		private static void BreakPost(PhysGrabObjectImpactDetector? __instance, float valueLost, Vector3 _contactPoint, int breakLevel)
		{
			ValuableObject? vo = __instance?.GetComponent<ValuableObject>();
			if (vo == null || PostLevelSummaryPlus.Instance == null) return;

			PostLevelSummaryPlus.Instance.Level.CheckValueChange(vo, valueLost);
		}
    }
}
