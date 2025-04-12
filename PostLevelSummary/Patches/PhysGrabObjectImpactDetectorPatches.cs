using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Unity.VisualScripting;
using UnityEngine;

namespace PostLevelSummary.Patches
{
    [HarmonyPatch(typeof(PhysGrabObjectImpactDetector))]
    class PhysGrabObjectImpactDetectorPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(PhysGrabObjectImpactDetector.BreakRPC))]
		static void BreakPost(PhysGrabObjectImpactDetector? __instance, float valueLost, Vector3 _contactPoint, int breakLevel, bool _loseValue)
		{
			ValuableObject? vo = __instance?.GetComponent<ValuableObject>();
			if (vo == null) return;

			PostLevelSummary.Level.CheckValueChangeFixed(vo, valueLost);
		}
    }
}
