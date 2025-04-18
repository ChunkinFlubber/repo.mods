using HarmonyLib;

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
