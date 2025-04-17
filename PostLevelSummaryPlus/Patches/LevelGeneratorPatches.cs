using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Photon.Pun;

namespace PostLevelSummaryPlus.Patches
{
    [HarmonyPatch(typeof(LevelGenerator))]
    class LevelGeneratorPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(LevelGenerator.GenerateDone))]
        public static void GenerateDonePostfix()
        {
            PostLevelSummaryPlus.LevelGenerationDone();
        }
    }
}
